using System.Windows.Forms;
using InfoFenix.Core;

namespace InfoFenix.Client.Code {

    public static class DialogHelper {

        #region Public Static Methods

        public static string OpenFolderBrowserDialog(string description, string startPath = null) {
            using (var folderBrowserDialog = new FolderBrowserDialog()) {
                folderBrowserDialog.Description = description;
                folderBrowserDialog.SelectedPath = (startPath ?? typeof(EntryPoint).Assembly.GetDirectoryPath());

                var result = folderBrowserDialog.ShowDialog();

                if (result != DialogResult.OK) { return null; }

                return folderBrowserDialog.SelectedPath;
            }
        }

        #endregion Public Static Methods
    }
}