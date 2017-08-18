namespace InfoFenix.Services {

    public interface IManagementService {

        #region Methods
        
        void BackupDatabase(string outputPath);

        void RestoreDatabase(string inputPath);

        void CleanUpDatabase();

        #endregion Methods
    }
}