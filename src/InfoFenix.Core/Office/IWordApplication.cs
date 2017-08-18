using System.IO;

namespace InfoFenix.Office {

    public interface IWordApplication {

        #region Methods

        IWordDocument Open(Stream stream);

        IWordDocument Open(string filePath);

        void CloseAllDocuments();

        void Quit();

        #endregion Methods
    }
}