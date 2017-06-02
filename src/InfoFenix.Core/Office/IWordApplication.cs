using System.IO;

namespace InfoFenix.Core.Office {

    public interface IWordApplication {

        #region Methods

        IWordDocument Open(Stream stream);

        IWordDocument Open(string filePath);

        void Quit();

        #endregion Methods
    }
}