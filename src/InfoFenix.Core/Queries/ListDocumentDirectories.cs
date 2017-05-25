using System.Collections.Generic;
using System.Data;
using InfoFenix.Core.Cqrs;
using InfoFenix.Core.Data;
using InfoFenix.Core.Entities;
using SQL = InfoFenix.Core.Resources.Resources;

namespace InfoFenix.Core.Queries {

    public class ListDocumentDirectoriesQuery : IQuery<IEnumerable<DocumentDirectoryEntity>> {

        #region Public Properties

        public string Label { get; set; }

        public string Path { get; set; }

        public string Code { get; set; }

        public bool Watch { get; set; }

        public bool Index { get; set; }

        #endregion Public Properties
    }

    public class ListDocumentDirectoriesQueryHandler : IQueryHandler<ListDocumentDirectoriesQuery, IEnumerable<DocumentDirectoryEntity>> {

        #region Private Read-Only Fields

        private readonly IDatabase _database;

        #endregion Private Read-Only Fields

        #region Public Constructors

        public ListDocumentDirectoriesQueryHandler(IDatabase database) {
            Prevent.ParameterNull(database, nameof(database));

            _database = database;
        }

        #endregion Public Constructors

        #region IQueryHandler<ListDocumentDirectoriesQuery, IEnumerable<DocumentDirectoryEntity>> Members

        public IEnumerable<DocumentDirectoryEntity> Handle(ListDocumentDirectoriesQuery query) {
            return _database.ExecuteReader(SQL.ListDocumentDirectories, DocumentDirectoryEntity.MapFromDataReader, parameters: new[] {
                Parameter.CreateInputParameter(nameof(query.Label), query.Label),
                Parameter.CreateInputParameter(nameof(query.Path), query.Path),
                Parameter.CreateInputParameter(nameof(query.Code), query.Code),
                Parameter.CreateInputParameter(nameof(query.Watch), query.Watch ? 1 : 0, DbType.Int32),
                Parameter.CreateInputParameter(nameof(query.Index), query.Index ? 1 : 0, DbType.Int32)
            });
        }

        #endregion IQueryHandler<ListDocumentDirectoriesQuery, IEnumerable<DocumentDirectoryEntity>> Members
    }
}