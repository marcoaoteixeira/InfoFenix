using System;
using System.Data;
using InfoFenix.Core.Cqrs;
using InfoFenix.Core.Data;
using InfoFenix.Core.Entities;
using InfoFenix.Core.Logging;
using SQL = InfoFenix.Core.Resources.Resources;

namespace InfoFenix.Core.Commands {

    public sealed class SaveDocumentDirectoryCommand : ICommand {

        #region Public Properties

        public int ID { get; set; }

        public string Label { get; set; }

        public string Path { get; set; }

        public string Code { get; set; }

        public bool Watch { get; set; }

        public bool Index { get; set; }

        #endregion Public Properties
    }

    public sealed class SaveDocumentDirectoryCommandHandler : ICommandHandler<SaveDocumentDirectoryCommand> {

        #region Private Read-Only Fields

        private readonly IDatabase _database;

        #endregion Private Read-Only Fields

        #region Public Properties

        private ILogger _log;

        public ILogger Log {
            get { return _log ?? NullLogger.Instance; }
            set { _log = value ?? NullLogger.Instance; }
        }

        #endregion Public Properties

        #region Public Constructors

        public SaveDocumentDirectoryCommandHandler(IDatabase database) {
            Prevent.ParameterNull(database, nameof(database));

            _database = database;
        }

        #endregion Public Constructors

        #region ICommandHandler<SaveDocumentDirectoryCommand> Members

        public void Handle(SaveDocumentDirectoryCommand command) {
            using (var transaction = _database.Connection.BeginTransaction()) {
                try {
                    var id = _database.ExecuteScalar(SQL.SaveDocumentDirectory, parameters: new[] {
                        Parameter.CreateInputParameter(nameof(DocumentDirectoryEntity.ID), command.ID != 0 ? (object)command.ID : DBNull.Value, DbType.Int32),
                        Parameter.CreateInputParameter(nameof(DocumentDirectoryEntity.Label), command.Label),
                        Parameter.CreateInputParameter(nameof(DocumentDirectoryEntity.Path), command.Path),
                        Parameter.CreateInputParameter(nameof(DocumentDirectoryEntity.Code), command.Code),
                        Parameter.CreateInputParameter(nameof(DocumentDirectoryEntity.Watch), command.Watch ? 1 : 0, DbType.Int32),
                        Parameter.CreateInputParameter(nameof(DocumentDirectoryEntity.Index), command.Index ? 1 : 0, DbType.Int32)
                    });
                    if (command.ID <= 0) { command.ID = Convert.ToInt32(id); }
                    transaction.Commit();
                } catch (Exception ex) { Log.Error(ex, ex.Message); transaction.Rollback(); throw; }
            }
        }

        #endregion ICommandHandler<SaveDocumentDirectoryCommand> Members
    }
}