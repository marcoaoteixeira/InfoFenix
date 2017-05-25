using System.IO;

namespace InfoFenix.Core.Office {

    public interface IWordDocumentService {

        #region Methods

        IWordDocument Open(Stream stream);

        IWordDocument Open(string filePath);

        #endregion Methods
    }
}