using System.Windows.Forms;
using InfoFenix;

namespace InfoFenix.Application.Code.Helpers {

    public static class DialogHelper {

        #region Public Static Methods

        public static string OpenFolderBrowserDialog(string description, string initialDirectory = null) {
            using (var dialog = new FolderBrowserDialog()) {
                dialog.Description = description;
                dialog.SelectedPath = (initialDirectory ?? Common.ApplicationDirectoryPath);

                var result = dialog.ShowDialog();

                if (result != DialogResult.OK) { return null; }

                return dialog.SelectedPath;
            }
        }

        public static string[] OpenFileBrowserDialog(string title, bool multiSelect = false, string initialDirectory = null) {
            using (var dialog = new OpenFileDialog()) {
                dialog.InitialDirectory = (initialDirectory ?? Common.ApplicationDirectoryPath);
                dialog.Title = title;
                dialog.Multiselect = multiSelect;

                var result = dialog.ShowDialog();
                if (result != DialogResult.OK) { return null; }

                return dialog.FileNames;
            }
        }

        #endregion Public Static Methods
    }
}