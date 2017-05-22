namespace InfoFenix.Core.Office {

    public interface IMicrosoftWordApplication {

        #region Methods

        IMicrosoftWordDocument Open(string filePath);

        void Quit();

        #endregion Methods
    }
}