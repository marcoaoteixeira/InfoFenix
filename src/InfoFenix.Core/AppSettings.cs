using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace InfoFenix.Core {
    public static class AppSettingsManager {

        #region Public Static Read-Only Fields

        public static readonly string AppSettingsFilePath = Path.Combine(Common.ApplicationDirectoryPath, "App.settings");

        #endregion Public Static Read-Only Fields

        #region Public Static Methods

        public static IAppSettings Get() {
            if (!File.Exists(AppSettingsFilePath)) { return AppSettingsImpl.Default; }

            using (var streamReader = new StreamReader(AppSettingsFilePath, encoding: Encoding.UTF8))
            using (var jsonTextReader = new JsonTextReader(streamReader)) {
                return new JsonSerializer().Deserialize<AppSettingsImpl>(jsonTextReader);
            }
        }

        public static void Save(IAppSettings appSettings) {
            using (var streamWriter = new StreamWriter(AppSettingsFilePath, append: false, encoding: Encoding.UTF8))
            using (var jsonTextWriter = new JsonTextWriter(streamWriter)) {
                new JsonSerializer().Serialize(jsonTextWriter, appSettings);
            }
        }

        #endregion Public Static Methods
    }

    public interface IAppSettings {

        #region Events

        event Action NotifyChange;

        #endregion Events

        #region Properties

        bool UseRemoteSearchDatabase { get; set; }
        string ApplicationDataDirectoryPath { get; set; }
        int LastOfficeWordProcessID { get; set; }

        #endregion Properties

        #region Methods

        void Save();

        #endregion Methods
    }

    public sealed class AppSettingsImpl : IAppSettings {

        #region Public Static Read-Only Fields

        [JsonIgnore]
        public static readonly IAppSettings Default = new AppSettingsImpl();

        #endregion Public Static Read-Only Fields

        #region Public Events

        public event Action NotifyChange;

        #endregion Public Events

        #region Private Fields

        private bool _useRemoteSearchDatabase = false;
        private string _applicationDataDirectoryPath = Common.DefaultAppDataDirectoryPath;
        private int _lastOfficeWordProcessID = int.MaxValue;

        #endregion Private Fields

        #region Public Properties

        [JsonProperty("useRemoteSearchDatabase")]
        public bool UseRemoteSearchDatabase {
            get { return _useRemoteSearchDatabase; }
            set {
                var actual = _useRemoteSearchDatabase;
                _useRemoteSearchDatabase = value;
                if (actual != value) {
                    OnChangeNotify();
                }
            }
        }

        [JsonProperty("applicationDataDirectoryPath")]
        public string ApplicationDataDirectoryPath {
            get {
                return _useRemoteSearchDatabase
                    ? _applicationDataDirectoryPath
                    : Common.DefaultAppDataDirectoryPath;
            }
            set {
                var actual = _applicationDataDirectoryPath;
                _applicationDataDirectoryPath = value;
                if (actual != value) {
                    OnChangeNotify();
                }
            }
        }

        [JsonProperty("lastOfficeWordProcessID")]
        public int LastOfficeWordProcessID {
            get { return _lastOfficeWordProcessID; }
            set {
                var actual = _lastOfficeWordProcessID;
                _lastOfficeWordProcessID = value;
                if (actual != value) {
                    OnChangeNotify();
                }
            }
        }

        public void Save() {
            AppSettingsManager.Save(this);
        }

        #endregion Public Properties

        #region Private Properties

        private void OnChangeNotify() {
            NotifyChange?.Invoke();
        }

        #endregion Private Properties
    }
}