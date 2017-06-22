namespace InfoFenix.Core.Services {

    public interface IManagementService {

        #region Methods
        
        void BackupDatabase(string outputPath);

        void RestoreDatabase(string inputPath);

        #endregion Methods
    }
}