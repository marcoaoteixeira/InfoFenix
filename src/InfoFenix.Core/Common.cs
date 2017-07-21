using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace InfoFenix.Core {

    public static class Common {

        #region Public Static Read-Only Fields

        public static readonly string ApplicationDirectoryPath = typeof(Common).Assembly.GetDirectoryPath();
        public static readonly string DefaultAppDataDirectoryPath = Path.Combine(ApplicationDirectoryPath, "App_Data");
        public static readonly string DatabaseFileName = "InfoFenix.s3db";
        public static readonly string IndexStorageDirectoryName = "IndexStorage";

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

        #endregion Public Static Methods

        #region Public Static Internal Classes

        public static class DocumentIndex {

            #region Public Static Read-Only Fields

            public static readonly string DefaultName = "INFO_FENIX_DOCUMENT_INDEX";

            #endregion Public Static Read-Only Fields

            #region Public Static Inner Classes

            public static class FieldNames {

                #region Public Static Read-Only Fields

                public static readonly string Content = nameof(Content);
                public static readonly string DocumentDirectoryCode = nameof(DocumentDirectoryCode);
                public static readonly string DocumentCode = nameof(DocumentCode);
                public static readonly string FileName = nameof(FileName);

                #endregion Public Static Read-Only Fields
            }

            #endregion Public Static Inner Classes
        }

        public static class DatabaseSchema {

            #region Public Static Inner Classes

            public static class Documents {

                #region Public Static Read-Only Fields

                public static readonly string DocumentID = "id";
                public static readonly string DocumentDirectoryID = "document_directory_id";
                public static readonly string Path = "path";
                public static readonly string LastWriteTime = "last_write_time";
                public static readonly string Code = "code";
                public static readonly string Indexed = "indexed";
                public static readonly string Content = "content";
                public static readonly string Payload = "payload";

                #endregion Public Static Read-Only Fields
            }

            public static class DocumentDirectories {

                #region Public Static Read-Only Fields

                public static readonly string DocumentDirectoryID = "id";
                public static readonly string Label = "label";
                public static readonly string Path = "path";
                public static readonly string Code = "code";
                public static readonly string Watch = "watch";
                public static readonly string Index = "index";

                #endregion Public Static Read-Only Fields
            }

            #endregion Public Static Inner Classes
        }

        #endregion Public Static Internal Classes
    }
}