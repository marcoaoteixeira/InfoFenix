using System.IO;

namespace InfoFenix.Core.IO {

    public static class IOHelper {

        #region Public Static Methods

        public static bool IsDirectory(string path) {
            return !string.IsNullOrWhiteSpace(path)
                ? IsWhat(path, FileAttributes.Directory)
                : false;
        }

        public static bool IsFile(string path) {
            return !string.IsNullOrWhiteSpace(path)
                ? IsWhat(path, FileAttributes.Archive)
                : false;
        }

        #endregion Public Static Methods

        #region Private Static Methods

        private static bool IsWhat(string path, FileAttributes attributes) {
            var result = false;
            try { result = File.GetAttributes(path).HasFlag(attributes); }
            catch (PathTooLongException) { /* ERROR: return false */ }
            catch (FileNotFoundException) { /* ERROR: return false */ }
            catch (DirectoryNotFoundException) { /* ERROR: return false */ }
            return result;
        }

        #endregion Private Static Methods
    }
}