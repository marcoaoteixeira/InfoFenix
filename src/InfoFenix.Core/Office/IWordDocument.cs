using System;

namespace InfoFenix.Core.Office {

    public interface IWordDocument : IDisposable {

        #region Properties

        string Text { get; }

        #endregion Properties

        #region Methods

        void Convert(string outputPath, WordConvertType type);

        void Close();

        #endregion Methods
    }
}