using System;
using System.Data;
using InfoFenix.Core.Cqrs;
using InfoFenix.Core.Data;
using InfoFenix.Core.Entities;
using InfoFenix.Core.Logging;
using SQL = InfoFenix.Core.Resources.Resources;

namespace InfoFenix.Core.Commands {

    public sealed class SetDocumentIndexCommand : ICommand {

        #region Public Properties

        public int ID { get; set; }

        #endregion Public Properties
    }

    public sealed class SetDocumentIndexCommandHandler : ICommandHandler<SetDocumentIndexCommand> {

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

        public SetDocumentIndexCommandHandler(IDatabase database) {
            Prevent.ParameterNull(database, nameof(database));

            _database = database;
        }

        #endregion Public Constructors

        #region ICommandHandler<SetDocumentIndexCommand> Members

        public void Handle(SetDocumentIndexCommand command) {
            using (var transaction = _database.Connection.BeginTransaction()) {
                try {
                    _database.ExecuteNonQuery(SQL.SetDocumentIndex, parameters: new[] {
                        Parameter.CreateInputParameter(nameof(DocumentEntity.ID), command.ID, DbType.Int32)
                    });
                    transaction.Commit();
                } catch (Exception ex) { Log.Error(ex, ex.Message); transaction.Rollback(); throw; }
            }
        }

        #endregion ICommandHandler<SetDocumentIndexCommand> Members
    }
}