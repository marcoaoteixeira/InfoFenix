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

        IEnumerable<DocumentEntity> GetDocuments(int documentDirectoryID);

        Task IndexAsync(int documentDirectoryID, CancellationToken cancellationToken);

        void StartWatchForModification(int documentDirectoryID);

        void StopWatchForModification(int documentDirectoryID);

        Task CleanAsync(int documentDirectoryID, CancellationToken cancellationToken);

        #endregion Methods
    }
}