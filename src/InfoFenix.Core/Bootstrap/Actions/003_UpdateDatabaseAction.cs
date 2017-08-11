using System;
using System.Data.SQLite;
using System.IO;
using InfoFenix.Core.Logging;
using Resource = InfoFenix.Core.Resources.Resources;

namespace InfoFenix.Core.Bootstrap.Actions {

    [Order(3)]
    public class UpdateDatabaseAction : ActionBase {

        #region Private Constants

        private const string UPDATE_KEY = "F53F6A9E-A619-4DB9-9B26-1026C5447148";

        #endregion Private Constants

        #region Public Properties

        private ILogger _log;

        /// <summary>
        /// Gets or sets the Logger value.
        /// </summary>
        public ILogger Logger {
            get { return _log ?? (_log = NullLogger.Instance); }
            set { _log = value ?? NullLogger.Instance; }
        }

        #endregion Public Properties

        #region IAction Members

        public override string Name => "Atualizando Banco de Dados";

        public override void Execute() {
            if (AppSettings.Instance.UseRemoteSearchDatabase) { return; }
            if (AppSettings.Instance.DatabaseUpdateKey == UPDATE_KEY) { return; }

            var databaseFilePath = Path.Combine(AppSettings.Instance.ApplicationDataDirectoryPath, Common.DatabaseFileName);
            using (var connection = new SQLiteConnection($"Data Source={databaseFilePath}; Version=3;", true)) {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                using (var command = new SQLiteCommand(connection)) {
                    try {
                        command.CommandText = Resource.UPDATE001_Rename_Tables;
                        command.ExecuteNonQuery();

                        command.CommandText = Resource.UPDATE002_Create_Structure;
                        command.ExecuteNonQuery();

                        command.CommandText = Resource.UPDATE003_Drop_Tables;
                        command.ExecuteNonQuery();

                        transaction.Commit();

                        AppSettings.Instance.DatabaseUpdateKey = UPDATE_KEY;
                        AppSettings.Instance.Save();
                    } catch (Exception ex) { Logger.Error("ERROR ON UPDATE DATABASE: {0}", ex.Message); transaction.Rollback(); throw; }
                }
            }
        }

        #endregion IAction Members
    }
}