using System.Collections.Generic;
using InfoFenix.Core.Cqrs;
using InfoFenix.Core.Dto;

namespace InfoFenix.Core.Queries {

    public class ListDocumentsNotInDatabaseQuery : IQuery<IEnumerable<string>> {

        #region Public Properties

        public DocumentDirectoryDto DocumentDirectory { get; set; }

        #endregion Public Properties
    }

    //public sealed class ListDocumentsNotInDatabaseQueryHandler : IQueryHandler<ListDocumentsNotInDatabaseQuery>
}