using System.Windows.Forms;
using InfoFenix.Core;

namespace InfoFenix.Client.Code {

    public static class DialogHelper {

        #region Public Static Methods

        public static string OpenFolderBrowserDialog(string description, string startPath = null) {
            using (var dialog = new FolderBrowserDialog()) {
                dialog.Description = description;
                dialog.SelectedPath = (startPath ?? typeof(EntryPoint).Assembly.GetDirectoryPath());

                var result = dialog.ShowDialog();

                if (result != DialogResult.OK) { return null; }

                return dialog.SelectedPath;
            }
        }

        public static string[] OpenFileBrowserDialog(string description, bool multiSelect = false, string startPath = null) {
            using (var dialog = new OpenFileDialog()) {
                dialog.Title = description;
                dialog.Multiselect = multiSelect;

                var result = dialog.ShowDialog();

                if (result != DialogResult.OK) { return null; }

                return dialog.FileNames;
            }
        }

        #endregion Public Static Methods
    }
}