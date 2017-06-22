using System.IO;

namespace InfoFenix.Core.Bootstrap.Actions {

    [Order(2)]
    public class CreateIndexStorageDirectoryAction : ActionBase {

        #region Private Read-Only Fields

        private readonly IAppSettings _appSettings;

        #endregion Private Read-Only Fields

        #region Public Constructors

        public CreateIndexStorageDirectoryAction(IAppSettings appSettings) {
            Prevent.ParameterNull(appSettings, nameof(appSettings));

            _appSettings = appSettings;
        }

        #endregion Public Constructors

        #region IAction Members

        public override string Name => "Criar Diretório de Índices";

        public override void Execute() {
            var indexStoragePath = Path.Combine(_appSettings.ApplicationDataDirectoryPath, Common.IndexStorageDirectoryName);
            if (!Directory.Exists(indexStoragePath)) {
                Directory.CreateDirectory(indexStoragePath);
            }
        }

        #endregion IAction Members
    }
}