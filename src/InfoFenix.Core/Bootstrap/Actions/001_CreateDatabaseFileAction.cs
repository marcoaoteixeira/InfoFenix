﻿using System.Data.SQLite;
using System.IO;
using SQL = InfoFenix.Core.Resources.Resources;

namespace InfoFenix.Core.Bootstrap.Actions {

    [Order(1)]
    public class CreateDatabaseFileAction : ActionBase {

        #region Private Read-Only Fields

        private readonly IAppSettings _appSettings;

        #endregion Private Read-Only Fields

        #region Public Constructors

        public CreateDatabaseFileAction(IAppSettings appSettings) {
            Prevent.ParameterNull(appSettings, nameof(appSettings));

            _appSettings = appSettings;
        }

        #endregion Public Constructors

        #region IAction Members

        public override void Execute() {
            var databaseFilePath = Path.Combine(_appSettings.ApplicationDataDirectoryPath, Common.DatabaseFileName);
            if (!File.Exists(databaseFilePath)) {
                SQLiteConnection.CreateFile(databaseFilePath);

                using (var connection = new SQLiteConnection($"Data Source={databaseFilePath}; Version=3;")) {
                    connection.Open();
                    using (var command = new SQLiteCommand(connection)) {
                        command.CommandText = SQL.CreateSchema;
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        #endregion IAction Members
    }
}