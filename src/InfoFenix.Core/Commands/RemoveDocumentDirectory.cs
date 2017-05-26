using System;
using System.Data;
using InfoFenix.Core.Cqrs;
using InfoFenix.Core.Data;
using InfoFenix.Core.Entities;
using InfoFenix.Core.Logging;
using InfoFenix.Core.Search;
using SQL = InfoFenix.Core.Resources.Resources;

namespace InfoFenix.Core.Commands {

    public sealed class RemoveDocumentDirectoryCommand : ICommand {

        #region Public Properties

        public int ID { get; set; }

        public string Label { get; set; }

        public string Code { get; set; }

        #endregion Public Properties
    }

    public sealed class RemoveDocumentDirectoryCommandHandler : ICommandHandler<RemoveDocumentDirectoryCommand> {

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

        public RemoveDocumentDirectoryCommandHandler(IDatabase database, IIndexProvider indexProvider) {
            Prevent.ParameterNull(database, nameof(database));
            Prevent.ParameterNull(indexProvider, nameof(indexProvider));

            _database = database;
            _indexProvider = indexProvider;
        }

        #endregion Public Constructors

        #region ICommandHandler<RemoveDocumentDirectoryCommand> Members

        public void Handle(RemoveDocumentDirectoryCommand command) {
            using (var transaction = _database.Connection.BeginTransaction()) {
                try {
                    var id = _database.ExecuteScalar(SQL.RemoveDocumentDirectory, parameters: new[] {
                        Parameter.CreateInputParameter(nameof(DocumentDirectoryEntity.ID), command.ID, DbType.Int32)
                    });
                    transaction.Commit();
                } catch (Exception ex) { Log.Error(ex, ex.Message); transaction.Rollback(); throw; }
            }

            try { _indexProvider.Delete(command.Code); }
            catch (Exception ex) { Log.Error(ex, $"Ocorreu um erro ao remover o índice \"{command.Label}\"."); }
        }

        #endregion ICommandHandler<RemoveDocumentDirectoryCommand> Members
    }
}