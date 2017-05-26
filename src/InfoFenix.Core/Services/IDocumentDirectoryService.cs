using System.Collections.Generic;
using InfoFenix.Core.Entities;

namespace InfoFenix.Core.Services {

    public interface IDocumentDirectoryService {

        #region Methods

        void Save(DocumentDirectoryEntity documentDirectory, bool saveDocumentsInsideDirectory = false);

        void Remove(int documentDirectoryID);

        DocumentDirectoryEntity Get(int documentDirectoryID);

        IEnumerable<DocumentDirectoryEntity> List(string label = null, string path = null, string code = null);

        void Index(int documentDirectoryID, bool reCreate = false);

        void WatchForModification(int documentDirectoryID);

        #endregion Methods
    }
}