using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using InfoFenix.Core.Cqrs;
using InfoFenix.Core.Data;
using InfoFenix.Core.Entities;
using InfoFenix.Core.Logging;
using SQL = InfoFenix.Core.Resources.Resources;

namespace InfoFenix.Core.Commands {

    public sealed class SaveDocumentCommand : ICommand {

        #region Public Properties

        public int ID { get; set; }

        public int DocumentDirectoryID { get; set; }

        public string Path { get; set; }

        public DateTime LastWriteTime { get; set; }

        public int Code { get; set; }

        public bool Indexed { get; set; }

        public byte[] Payload { get; set; }

        #endregion Public Properties
    }

    public sealed class SaveDocumentCommandHandler : ICommandHandler<SaveDocumentCommand> {

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

        public SaveDocumentCommandHandler(IDatabase database) {
            Prevent.ParameterNull(database, nameof(database));

            _database = database;
        }

        #endregion Public Constructors

        #region ICommandHandler<SaveDocumentCommand> Members

        public Task HandleAsync(SaveDocumentCommand command, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => {
                using (var transaction = _database.Connection.BeginTransaction()) {
                    try {
                        var id = _database.ExecuteScalar(SQL.SaveDocument, parameters: new[] {
                        Parameter.CreateInputParameter(nameof(DocumentEntity.ID), command.ID > 0 ? (object)command.ID : DBNull.Value, DbType.Int32),
                        Parameter.CreateInputParameter(nameof(DocumentEntity.DocumentDirectoryID), command.DocumentDirectoryID, DbType.Int32),
                        Parameter.CreateInputParameter(nameof(DocumentEntity.Path), command.Path),
                        Parameter.CreateInputParameter(nameof(DocumentEntity.LastWriteTime), command.LastWriteTime, DbType.DateTime),
                        Parameter.CreateInputParameter(nameof(DocumentEntity.Code), command.Code, DbType.Int32),
                        Parameter.CreateInputParameter(nameof(DocumentEntity.Indexed), command.Indexed ? 1 : 0, DbType.Int32),
                        Parameter.CreateInputParameter(nameof(DocumentEntity.Payload), command.Payload != null ? (object)command.Payload : DBNull.Value, DbType.Binary)
                    });
                        if (command.ID <= 0) { command.ID = Convert.ToInt32(id); }
                        transaction.Commit();
                    } catch (Exception ex) { Log.Error(ex, ex.Message); transaction.Rollback(); throw; }
                }
            });
        }

        #endregion ICommandHandler<SaveDocumentCommand> Members
    }
}