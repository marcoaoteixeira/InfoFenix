namespace InfoFenix.Core.Services {

    public interface IDocumentService {

        #region Methods

        void Index(int documentID);

        void Remove(int documentID);

        #endregion Methods
    }
}