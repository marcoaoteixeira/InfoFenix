using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using InfoFenix.Client.Code;
using InfoFenix.Client.Views.Shared;
using InfoFenix.Core;
using InfoFenix.Core.Cqrs;
using InfoFenix.Core.Entities;
using InfoFenix.Core.PubSub;
using InfoFenix.Impl.Queries;

namespace InfoFenix.Client.Views {
    public partial class ManageDocumentDirectoryForm : LayoutForm {
        #region Private Constants

        private const string GrayDocImageName = "file-doc_gray_32x32.png";
        private const string BlueDocImageName = "file-doc_blue_32x32.png";
        private const string GrayDocxImageName = "file-docx_gray_32x32.png";
        private const string BlueDocxImageName = "file-docx_blue_32x32.png";

        #endregion

        #region Private Read-Only Fields

        private readonly ICqrsDispatcher _cqrsDispatcher;
        private readonly IFormManager _formManager;
        private readonly IPublisherSubscriber _pubSub;

        #endregion

        #region Private Fields

        private ISubscription<DirectoryContentChangeNotification> _directoryContentChangeNotificationSubscription;

        #endregion

        #region Public Constructors
        public ManageDocumentDirectoryForm(ICqrsDispatcher cqrsDispatcher, IFormManager formManager, IPublisherSubscriber pubSub) {
            Prevent.ParameterNull(cqrsDispatcher, nameof(cqrsDispatcher));
            Prevent.ParameterNull(formManager, nameof(formManager));
            Prevent.ParameterNull(pubSub, nameof(pubSub));

            _cqrsDispatcher = cqrsDispatcher;
            _formManager = formManager;
            _pubSub = pubSub;

            InitializeComponent();
        }
        #endregion

        #region Event Handlers
        private void addDirectoryButton_Click(object sender, EventArgs e) {
            var button = sender as Button;
            if (button == null) { return; }

            using (var dialog = _formManager.Get<DocumentDirectoryForm>()) {
                if (dialog.ShowDialog() != DialogResult.OK) { return; }

                var documentDirectory = dialog.Tag as DocumentDirectoryEntity;
                if (documentDirectory == null) { return; }

                CreateTreeViewEntry(documentDirectory);
            }
        }
        #endregion

        #region Private Methods

        private void Initialize() {
            documentDirectoryListView.StateImageList = manageDocumentDirectoryImageList;
        }

        private void SubscribeForNotification() {
            _directoryContentChangeNotificationSubscription = _pubSub.Subscribe<DirectoryContentChangeNotification>(DirectoryContentChangeHandler);
        }

        private void UnsubscribeForNotification() {
            _pubSub.Unsubscribe(_directoryContentChangeNotificationSubscription);
        }

        private void DirectoryContentChangeHandler(DirectoryContentChangeNotification message) {
            
        }

        private void LoadDirectoryTreeView() {
            var documentDirectories = _cqrsDispatcher.Query(new ListDocumentDirectoriesQuery());

            foreach (var documentDirectory in documentDirectories) {
                CreateTreeViewEntry(documentDirectory); 
            }
        }

        private void CreateTreeViewEntry(DocumentDirectoryEntity documentDirectory) {
            directoryTreeView.Nodes.Add(new TreeNode {
                Text = documentDirectory.Label,
                Tag = documentDirectory
            });
        }

        private void removeDirectoryToolStripMenuItem_Click(object sender, EventArgs e) {
            var node = directoryTreeView.SelectedNode;

            if (node == null) { return; }

            MessageBox.Show(node.Text);
        }

        private void directoryTreeView_MouseUp(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Right) {
                directoryTreeView.SelectedNode = directoryTreeView.GetNodeAt(e.X, e.Y);
                if (directoryTreeView.SelectedNode != null) {
                    directoryTreeViewContextMenuStrip.Show(directoryTreeView, e.Location);
                }
            }
        }

        #endregion

        private void directoryTreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e) {
            var documentDirectory = e.Node.Tag as DocumentDirectoryEntity;
            if (documentDirectory == null) { return; }

            var documents = _cqrsDispatcher.Query(new ListDocumentsByDocumentDirectoryQuery { DocumentDirectoryID = documentDirectory.DocumentDirectoryID });
            foreach (var document in documents) {
                documentDirectoryListView.Items.Add(new ListViewItem {
                    Text = document.FileName,
                    ImageIndex = GetImageListIndex(document)
                });
            }
        }

        private int GetImageListIndex(DocumentEntity document) {
            return document.Indexed
                ? document.FullPath.EndsWith(".docx", StringComparison.InvariantCultureIgnoreCase) ? 3 : 1
                : document.FullPath.EndsWith(".doc", StringComparison.InvariantCultureIgnoreCase) ? 2 : 0;
        }

        private void ManageDocumentDirectoryForm_Load(object sender, EventArgs e) {
            Initialize();
        }
    }
}
