using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using InfoFenix.Core.Cqrs;
using InfoFenix.Core.Data;
using InfoFenix.Core.Entities;
using InfoFenix.Core.Logging;
using InfoFenix.Core.Search;
using SQL = InfoFenix.Core.Resources.Resources;

namespace InfoFenix.Core.Commands {

    public sealed class RemoveDocumentCommand : ICommand {

        #region Public Properties

        public int ID { get; set; }

        public string FileName { get; set; }

        public string DocumentDirectoryCode { get; set; }

        #endregion Public Properties
    }

    public sealed class RemoveDocumentCommandHandler : ICommandHandler<RemoveDocumentCommand> {

        #region Private Read-Only Fields

        private readonly IDatabase _database;
        private readonly IIndexProvider _indexProvider;

        #endregion Private Read-Only Fields

        #region Public Properties

        private ILogger _log;

        public ILogger Log {
            get { return _log ?? NullLogger.Instance; }
            set { _log = value ?? NullLogger.Instance; }
        }

        #endregion Public Properties

        #region Public Constructors

        public RemoveDocumentCommandHandler(IDatabase database, IIndexProvider indexProvider) {
            Prevent.ParameterNull(database, nameof(database));
            Prevent.ParameterNull(indexProvider, nameof(indexProvider));

            _database = database;
            _indexProvider = indexProvider;
        }

        #endregion Public Constructors

        #region ICommandHandler<RemoveDocumentCommand> Members

        public Task HandleAsync(RemoveDocumentCommand command, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => {
                using (var transaction = _database.Connection.BeginTransaction()) {
                    try {
                        _database.ExecuteScalar(SQL.RemoveDocument, parameters: new[] {
                            Parameter.CreateInputParameter(nameof(DocumentEntity.ID), command.ID, DbType.Int32)
                        });
                        transaction.Commit();
                    } catch (Exception ex) { Log.Error(ex, ex.Message); transaction.Rollback(); throw; }
                }

                try {
                    _indexProvider
                        .GetOrCreate(command.DocumentDirectoryCode)
                        .DeleteDocuments(command.ID.ToString());
                } catch (Exception ex) { Log.Error(ex, $"Erro ao remover o documento {command.FileName} do índice."); }
            });
        }

        #endregion ICommandHandler<RemoveDocumentCommand> Members
    }
}