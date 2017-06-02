using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using InfoFenix.Core.Entities;

namespace InfoFenix.Core.Services {

    public interface IDocumentDirectoryService {

        #region Methods

        void Save(DocumentDirectoryEntity documentDirectory);

        Task SaveDocumentsInsideDocumentDirectoryAsync(int documentDirectoryID, CancellationToken cancellationToken);

        void Remove(int documentDirectoryID);

        DocumentDirectoryEntity Get(int documentDirectoryID);

        IEnumerable<DocumentDirectoryEntity> List(string label = null, string path = null, string code = null);

        Task IndexAsync(int documentDirectoryID, CancellationToken cancellationToken);

        void WatchForModification(int documentDirectoryID);

        Task CleanAsync(int documentDirectoryID, CancellationToken cancellationToken);

        #endregion Methods
    }
}