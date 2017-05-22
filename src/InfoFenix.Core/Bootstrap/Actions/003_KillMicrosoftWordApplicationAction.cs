using System.Diagnostics;

namespace InfoFenix.Core.Bootstrap.Actions {

    [Order(3)]
    public sealed class KillMicrosoftWordApplicationAction : ActionBase {

        #region Private Read-Only Fields

        private readonly IAppSettings _appSettings;

        #endregion Private Read-Only Fields

        #region Public Constructors

        public KillMicrosoftWordApplicationAction(IAppSettings appSettings) {
            Prevent.ParameterNull(appSettings, nameof(appSettings));

            _appSettings = appSettings;
        }

        #endregion Public Constructors

        #region Public Override Methods

        public override void Execute() {
            if (_appSettings.LastOfficeWordProcessID <= 0 || _appSettings.LastOfficeWordProcessID == int.MaxValue) { return; }

            try {
                using (var process = Process.GetProcessById(_appSettings.LastOfficeWordProcessID)) {
                    if (process == null) { return; }
                    if (process.HasExited) { return; }
                    if (process.ProcessName != "WINWORD") { return; }

                    process.Kill();
                    process.WaitForExit();
                }
            }
            catch { }
            finally {
                _appSettings.LastOfficeWordProcessID = int.MaxValue;
                _appSettings.Save();
            }
        }

        #endregion Public Override Methods
    }
}