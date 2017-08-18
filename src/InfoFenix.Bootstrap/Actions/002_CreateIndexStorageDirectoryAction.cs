using System.IO;

namespace InfoFenix.Bootstrap.Actions {

    [Order(2)]
    public class CreateIndexStorageDirectoryAction : ActionBase {

        #region IAction Members

        public override string Name => "Criar Diretório de Índices";

        public override void Execute() {
            if (AppSettings.Instance.UseRemoteSearchDatabase) { return; }

            var indexStoragePath = Path.Combine(AppSettings.Instance.ApplicationDataDirectoryPath, Common.IndexStorageDirectoryName);
            if (!Directory.Exists(indexStoragePath)) {
                Directory.CreateDirectory(indexStoragePath);
            }
        }

        #endregion IAction Members
    }
}