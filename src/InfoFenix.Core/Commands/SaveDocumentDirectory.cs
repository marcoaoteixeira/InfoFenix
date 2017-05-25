using System;
using System.Data;
using InfoFenix.Core.Cqrs;
using InfoFenix.Core.Data;
using InfoFenix.Core.Entities;
using InfoFenix.Core.Logging;
using InfoFenix.Core.PubSub;
using SQL = InfoFenix.Core.Resources.Resources;

namespace InfoFenix.Core.Commands {

    public class SaveDocumentDirectoryCommand : ICommand {

        #region Public Properties

        public int DocumentDirectoryID { get; set; }

        public string Label { get; set; }

        public string DirectoryPath { get; set; }

        public string Code { get; set; }

        public bool Watch { get; set; }

        public bool Index { get; set; }

        #endregion Public Properties
    }

    public class SaveDocumentDirectoryCommandHandler : ICommandHandler<SaveDocumentDirectoryCommand> {

        #region Private Read-Only Fields

        private readonly IDatabase _database;
        private readonly IPublisherSubscriber _pubSub;

        #endregion Private Read-Only Fields

        #region Public Properties

        private ILogger _log;

        public ILogger Log {
            get { return _log ?? NullLogger.Instance; }
            set { _log = value ?? NullLogger.Instance; }
        }

        #endregion Public Properties

        #region Public Constructors

        public SaveDocumentDirectoryCommandHandler(IDatabase database, IPublisherSubscriber pubSub) {
            Prevent.ParameterNull(database, nameof(database));
            Prevent.ParameterNull(pubSub, nameof(pubSub));

            _database = database;
            _pubSub = pubSub;
        }

        #endregion Public Constructors

        #region ICommandHandler<SaveDocumentDirectoryCommand> Members

        public void Handle(SaveDocumentDirectoryCommand command) {
            using (var transaction = _database.Connection.BeginTransaction()) {
                try {
                    var output = _database.ExecuteScalar(SQL.SaveDocumentDirectory, parameters: new[] {
                        Parameter.CreateInputParameter(nameof(DocumentDirectoryEntity.ID), command.DocumentDirectoryID > 0 ? (object)command.DocumentDirectoryID : DBNull.Value, DbType.Int32),
                        Parameter.CreateInputParameter(nameof(DocumentDirectoryEntity.Label), command.Label),
                        Parameter.CreateInputParameter(nameof(DocumentDirectoryEntity.Path), command.DirectoryPath),
                        Parameter.CreateInputParameter(nameof(DocumentDirectoryEntity.Code), command.Code),
                        Parameter.CreateInputParameter(nameof(DocumentDirectoryEntity.Watch), command.Watch == true ? 1 : 0, DbType.Int32),
                        Parameter.CreateInputParameter(nameof(DocumentDirectoryEntity.Index), command.Index == true ? 1 : 0, DbType.Int32)
                    });
                    if (command.DocumentDirectoryID <= 0) { command.DocumentDirectoryID = Convert.ToInt32(output); }
                    transaction.Commit();
                } catch (Exception ex) {
                    Log.Error(ex, ex.Message);
                    transaction.Rollback();
                    throw;
                }
            }
        }

        #endregion ICommandHandler<SaveDocumentDirectoryCommand> Members
    }
}