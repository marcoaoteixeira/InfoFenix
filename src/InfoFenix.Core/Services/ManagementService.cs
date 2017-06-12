using System;
using System.IO;
using System.IO.Compression;
using InfoFenix.Core.Data;
using InfoFenix.Core.IO;
using InfoFenix.Core.Search;

namespace InfoFenix.Core.Services {

    public class ManagementService : IManagementService {

        #region Private Read-Only Fields

        private readonly IAppSettings _appSettings;
        private readonly IDatabase _database;
        private readonly IIndexProvider _indexProvider;

        #endregion Private Read-Only Fields

        #region Public Constructors

        public ManagementService(IAppSettings appSettings, IDatabase database, IIndexProvider indexProvider) {
            Prevent.ParameterNull(appSettings, nameof(appSettings));
            Prevent.ParameterNull(database, nameof(database));
            Prevent.ParameterNull(indexProvider, nameof(indexProvider));

            _appSettings = appSettings;
            _database = database;
            _indexProvider = indexProvider;
        }

        #endregion Public Constructors

        #region IManagementService Members

        public void BackupDatabase(string outputPath) {
            if (_appSettings.UseRemoteSearchDatabase) {
                throw new InvalidOperationException("Não é possível realizar o backup na máquina cliente.");
            }

            if (!IOHelper.IsDirectory(outputPath)) {
                throw new InvalidOperationException("O caminho selecionado deve apontar para um diretório válido.");
            }

            var tempDirectory = Path.Combine(Path.GetTempPath(), "InfoFenixBkp");

            // Now Create all of the directories
            foreach (var directory in Directory.GetDirectories(_appSettings.ApplicationDataDirectoryPath, "*", SearchOption.AllDirectories)) {
                Directory.CreateDirectory(directory.Replace(_appSettings.ApplicationDataDirectoryPath, tempDirectory));
            }

            // Copy all the files & Replaces any files with the same name
            foreach (var file in Directory.GetFiles(_appSettings.ApplicationDataDirectoryPath, "*.*", SearchOption.AllDirectories)) {
                File.Copy(file, file.Replace(_appSettings.ApplicationDataDirectoryPath, tempDirectory), true);
            }

            var filePath = Path.Combine(outputPath, string.Format("{0:yyyyMMdd_HHmmss}_info_fenix_db.zip", DateTime.Now));
            ZipFile.CreateFromDirectory(tempDirectory, filePath);

            // Removes temporary files.
            Directory.Delete(tempDirectory, recursive: true);
        }

        public void RestoreDatabase(string path) {
            if (_appSettings.UseRemoteSearchDatabase) {
                throw new InvalidOperationException("Não é possível realizar a restauração a partir da máquina cliente.");
            }

            if (!IOHelper.IsFile(path)) {
                throw new InvalidOperationException("O caminho selecionado deve apontar para um arquivo válido.");
            }

            // Close database
            (_database as IDisposable).Dispose();

            // Close all indexes
            foreach (var indexName in _indexProvider.List()) {
                (_indexProvider.GetOrCreate(indexName) as IDisposable).Dispose();
            }

            // Delete all current files
            Directory.Delete(_appSettings.ApplicationDataDirectoryPath, recursive: true);

            ZipFile.ExtractToDirectory(path, _appSettings.ApplicationDataDirectoryPath);
        }

        #endregion IManagementService Members
    }
}