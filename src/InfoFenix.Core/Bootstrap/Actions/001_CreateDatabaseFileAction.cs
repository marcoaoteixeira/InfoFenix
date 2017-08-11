using System.Data.SQLite;
using System.IO;
using Resource = InfoFenix.Core.Resources.Resources;

namespace InfoFenix.Core.Bootstrap.Actions {

    [Order(1)]
    public class CreateDatabaseFileAction : ActionBase {

        #region IAction Members

        public override string Name => "Criar Banco de Dados";

        public override void Execute() {
            if (AppSettings.Instance.UseRemoteSearchDatabase) { return; }

            var databaseFilePath = Path.Combine(AppSettings.Instance.ApplicationDataDirectoryPath, Common.DatabaseFileName);
            if (!File.Exists(databaseFilePath)) {
                SQLiteConnection.CreateFile(databaseFilePath);

                using (var connection = new SQLiteConnection($"Data Source={databaseFilePath}; Version=3;", true)) {
                    connection.Open();
                    using (var command = new SQLiteCommand(connection)) {
                        command.CommandText = Resource.CreateSchemaSQL;
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        #endregion IAction Members
    }
}