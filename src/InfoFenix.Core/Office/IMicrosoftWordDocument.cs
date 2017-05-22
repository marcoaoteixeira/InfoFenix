namespace InfoFenix.Core.Office {

    public interface IMicrosoftWordDocument {
        #region Methods

        string ReadContent();

        void Convert(string outputFilePath, WordConvertType type);

        void Close();

        #endregion Methods
    }
}