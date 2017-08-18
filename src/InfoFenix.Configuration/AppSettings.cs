using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace InfoFenix {

    /// <summary>
    /// Singleton Pattern implementation for AppSettings. (see: https://en.wikipedia.org/wiki/Singleton_pattern)
    /// </summary>
    public sealed class AppSettings {

        #region Public Static Read-Only Fields

        [JsonIgnore]
        public static readonly string AppSettingsFilePath = Path.Combine(Common.ApplicationDirectoryPath, "App.settings");

        #endregion Public Static Read-Only Fields

        #region Private Static Read-Only Fields

        private static readonly AppSettings _instance = new AppSettings();

        #endregion Private Static Read-Only Fields

        #region Public Properties

        [JsonProperty(nameof(UseRemoteSearchDatabase))]
        public bool UseRemoteSearchDatabase { get; set; } = false;

        [JsonProperty(nameof(ApplicationDataDirectoryPath))]
        public string ApplicationDataDirectoryPath { get; set; } = Path.Combine(Common.ApplicationDirectoryPath, "App_Data");

        #endregion Public Properties

        #region Public Static Properties

        /// <summary>
        /// Gets the unique instance of AppSettings.
        /// </summary>
        [JsonIgnore]
        public static AppSettings Instance {
            get { return _instance; }
        }

        #endregion Public Static Properties

        #region Static Constructors

        // Explicit static constructor to tell the C# compiler
        // not to mark type as beforefieldinit
        static AppSettings() {
        }

        #endregion Static Constructors

        #region Private Constructors

        private AppSettings() {
            Initialize();
        }

        #endregion Private Constructors

        #region Public Methods

        public void Save() {
            using (var streamWriter = new StreamWriter(AppSettingsFilePath, append: false, encoding: Encoding.UTF8))
            using (var jsonTextWriter = new JsonTextWriter(streamWriter)) {
                new JsonSerializer().Serialize(jsonTextWriter, this);
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void Initialize() {
            ReadJsonFile();
        }

        private void ReadJsonFile() {
            if (!File.Exists(AppSettingsFilePath)) { return; }

            using (var streamReader = new StreamReader(AppSettingsFilePath, encoding: Encoding.UTF8))
            using (var jsonTextReader = new JsonTextReader(streamReader)) {
                var json = (JObject)JToken.ReadFrom(jsonTextReader);

                UseRemoteSearchDatabase = json[nameof(UseRemoteSearchDatabase)].Value<bool>();
                ApplicationDataDirectoryPath = json[nameof(ApplicationDataDirectoryPath)].Value<string>();
            }
        }

        #endregion Private Methods
    }
}