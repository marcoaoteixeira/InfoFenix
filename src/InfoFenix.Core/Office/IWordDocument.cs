using System;
using System.IO;

namespace InfoFenix.Core.Office {

    public interface IWordDocument : IDisposable {

        #region Properties

        string Text { get; }

        #endregion Properties

        #region Methods

        void Convert(string outputPath, WordConvertType type);

        void Convert(Stream outputStream, WordConvertType type);

        void Highlight(Stream outputStream, params string[] terms);

        void Close();

        #endregion Methods
    }
}