using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace InfoFenix.Core {

    public static class Common {

        #region Public Static Read-Only Fields

        public static readonly IList<int> WordProcesses = new List<int>();

        public static readonly string ApplicationDirectoryPath = typeof(Common).Assembly.GetDirectoryPath();
        public static readonly string DefaultAppDataDirectoryPath = Path.Combine(ApplicationDirectoryPath, "App_Data");
        public static readonly string DatabaseFileName = "InformaNET.s3db";
        public static readonly string IndexStorageDirectoryName = "IndexStorage";

        public static readonly string TagDocumentDirectoryCode = "DocumentDirectoryCode";
        public static readonly string TagContent = "Content";

        #endregion Public Static Read-Only Fields

        #region Public Static Methods

        public static string[] GetDocFiles(string path) {
            return Directory.GetFiles(path, "*.doc", SearchOption.AllDirectories).Where(_ => !Path.GetFileName(_).StartsWith("~")).ToArray();
        }

        public static int ExtractCodeFromFilePath(string filePath) {
            var match = Regex.Match(Path.GetFileNameWithoutExtension(filePath), "([0-9]{1,})");
            return match.Captures.Count > 0 ? int.Parse(match.Captures[0].Value) : 0;
        }

        public static FileInfo[] GetDocFilesInfo(string path) {
            return GetDocFiles(path).Select(_ => new FileInfo(_)).ToArray();
        }

        /// <summary>
        /// Returns the name of that process given by that title
        /// </summary>
        /// <param name="mainWindowTitle">The window main title.</param>
        /// <returns><see cref="int.MaxValue"/> returned if it cant be found</returns>
        public static int GetProcessID(string mainWindowTitle) {
            var processes = Process.GetProcesses();
            for (var idx = 0; idx < processes.Length; idx++) {
                if (processes[idx].MainWindowTitle == mainWindowTitle) {
                    return processes[idx].Id;
                }
            }
            return int.MaxValue;
        }

        #endregion Public Static Methods

        #region Public Static Internal Classes

        public static class DocumentIndex {

            #region Public Static Read-Only Fields

            public static readonly string Content = nameof(Content);
            public static readonly string DocumentDirectoryCode = nameof(DocumentDirectoryCode);
            public static readonly string DocumentCode = nameof(DocumentCode);

            #endregion Public Static Read-Only Fields
        }

        #endregion Public Static Internal Classes
    }
}