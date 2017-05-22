using System;
using System.Data;
using InfoFenix.Core;
using InfoFenix.Core.Cqrs;
using InfoFenix.Core.Data;
using InfoFenix.Core.Entities;
using SQL = InfoFenix.Impl.Resources.Resources;

namespace InfoFenix.Impl.Commands {
    public class SaveDocumentDirectoryCommand : ICommand {
        #region Public Properties

        public int DocumentDirectoryID { get; set; }

        public string Label { get; set; }

        public string DirectoryPath { get; set; }

        public string Code { get; set; }

        public bool Watch { get; set; }

        public bool Index { get; set; }

        #endregion
    }

    public class SaveDocumentDirectoryCommandHandler : ICommandHandler<SaveDocumentDirectoryCommand> {
        #region Private Read-Only Fields

        private readonly IDatabase _database;

        #endregion

        #region Public Constructors

        public SaveDocumentDirectoryCommandHandler(IDatabase database) {
            Prevent.ParameterNull(database, nameof(database));

            _database = database;
        }

        #endregion

        #region ICommandHandler<SaveDocumentDirectoryCommand> Members
        public void Handle(SaveDocumentDirectoryCommand command) {
            var output = _database.ExecuteScalar(SQL.SaveDocumentDirectory, parameters: new[] {
                Parameter.CreateInputParameter(nameof(DocumentDirectoryEntity.DocumentDirectoryID), command.DocumentDirectoryID, DbType.Int32),
                Parameter.CreateInputParameter(nameof(DocumentDirectoryEntity.Label), command.Label),
                Parameter.CreateInputParameter(nameof(DocumentDirectoryEntity.DirectoryPath), command.DirectoryPath),
                Parameter.CreateInputParameter(nameof(DocumentDirectoryEntity.Code), command.Code),
                Parameter.CreateInputParameter(nameof(DocumentDirectoryEntity.Watch), command.Watch == true ? 1 : 0, DbType.Int32),
                Parameter.CreateInputParameter(nameof(DocumentDirectoryEntity.Index), command.Index == true ? 1 : 0, DbType.Int32)
            });
            if (command.DocumentDirectoryID <= 0) { command.DocumentDirectoryID = Convert.ToInt32(output); }
        } 
        #endregion
    }
}
