using System.IO;

namespace InfoFenix.Core.Bootstrap.Actions {

    [Order(0)]
    public class CreateAppDataDirectoryAction : ActionBase {

        #region IAction Members

        public override string Name => "Criar Diretório de Dados";

        public override void Execute() {
            if (!Directory.Exists(AppSettings.Instance.ApplicationDataDirectoryPath)) {
                Directory.CreateDirectory(AppSettings.Instance.ApplicationDataDirectoryPath);
            }
        }

        #endregion IAction Members
    }
}