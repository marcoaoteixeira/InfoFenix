namespace InfoFenix.Core.Services {

    public interface IDocumentService {

        #region Methods

        void Index(int documentID);

        void Remove(int documentID);

        void Process(string documentDirectoryPath, string documentPath);

        #endregion Methods
    }
}