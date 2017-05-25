using System;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using InfoFenix.Client.Code;
using InfoFenix.Client.Views.Shared;
using InfoFenix.Core;
using InfoFenix.Core.Cqrs;
using InfoFenix.Core.Entities;
using InfoFenix.Core.PubSub;
using InfoFenix.Core.Queries;
using System.Collections;
using System.Collections.Generic;

namespace InfoFenix.Client.Views {

    public partial class ManageDocumentDirectoryForm : LayoutForm {

        #region Private Constants

        private const int FOLDER_BLUE_ICON_INDEX = 0;
        private const int WORD_DOC_BLUE_ICON_INDEX = 1;
        private const int WORD_DOC_GRAY_ICON_INDEX = 2;
        private const int WORD_DOCX_BLUE_ICON_INDEX = 3;
        private const int WORD_DOCX_GRAY_ICON_INDEX = 4;

        #endregion Private Constants

        #region Private Read-Only Fields

        private readonly ICqrsDispatcher _cqrsDispatcher;
        private readonly IFormManager _formManager;
        private readonly IPublisherSubscriber _pubSub;

        #endregion Private Read-Only Fields

        #region Private Properties

        private IEnumerable<DocumentEntity> _currentDocumentDirectoryItems;
        private IEnumerable<DocumentEntity> CurrentDocumentDirectoryItems {
            get { return _currentDocumentDirectoryItems ?? Enumerable.Empty<DocumentEntity>(); }
            set { _currentDocumentDirectoryItems = value ?? Enumerable.Empty<DocumentEntity>(); }
        }

        #endregion

        #region Private Fields

        private ISubscription<DirectoryContentChangeNotification> _directoryContentChangeNotificationSubscription;

        #endregion Private Fields

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

        #endregion Public Constructors

        #region Event Handlers

        private void ManageDocumentDirectoryForm_Load(object sender, EventArgs e) {
            Initialize();
        }

        private void addDirectoryButton_Click(object sender, EventArgs e) {
            var button = sender as Button;
            if (button == null) { return; }

            using (var dialog = _formManager.Get<DocumentDirectoryForm>()) {
                if (dialog.ShowDialog() != DialogResult.OK) { return; }
                if (dialog.ViewModel == null) { return; }

                CreateTreeViewEntry(dialog.ViewModel);
            }
        }

        private void directoryTreeView_MouseUp(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Right) {
                directoryTreeView.SelectedNode = directoryTreeView.GetNodeAt(e.X, e.Y);
                if (directoryTreeView.SelectedNode != null) {
                    directoryTreeViewContextMenuStrip.Show(directoryTreeView, e.Location);
                }
            }
        }

        private void directoryTreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e) {
            var documentDirectory = e.Node.Tag as DocumentDirectoryEntity;
            if (documentDirectory == null) { return; }

            CurrentDocumentDirectoryItems = _cqrsDispatcher.Query(new ListDocumentsByDocumentDirectoryQuery { DocumentDirectoryID = documentDirectory.ID });
            foreach (var document in CurrentDocumentDirectoryItems) {
                var text = Path.GetFileNameWithoutExtension(document.FileName);
                documentDirectoryListView.Items.Add(new ListViewItem {
                    Text = text,
                    ImageIndex = GetImageListIndex(document),
                    Name = text,
                    Tag = document,
                    ToolTipText = document.Indexed ? "Indexado" : "Não indexado"
                });
            }
            directoryInformationLabel.Text = $"Total de documentos: {CurrentDocumentDirectoryItems.Count()}";
        }

        private void removeDirectoryToolStripMenuItem_Click(object sender, EventArgs e) {
            var node = directoryTreeView.SelectedNode;

            if (node == null) { return; }

            MessageBox.Show(node.Text);
        }

        private void filterListViewTextBox_KeyDown(object sender, KeyEventArgs e) {
            var textBox = sender as TextBox;
            if (textBox == null) { return; }

            FilterListView(textBox.Text);
        }

        private void documentDirectoryListView_MouseDown(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Right) {
                var listViewItem = documentDirectoryListView.GetItemAt(e.X, e.Y);
                if (listViewItem != null) {
                    documentDirectoryListViewContextMenuStrip.Show(documentDirectoryListView, e.Location);
                }
            }
        }

        private void closeButton_Click(object sender, EventArgs e) {
            Close();
        }

        #endregion Event Handlers

        #region Private Methods

        private void Initialize() {
            directoryTreeView.ImageList = manageDocumentDirectoryImageList;
            documentDirectoryListView.LargeImageList = manageDocumentDirectoryImageList;
            documentDirectoryListView.SmallImageList = manageDocumentDirectoryImageList;

            LoadDirectoryTreeView();
        }

        private void SubscribeForNotification() {
            _directoryContentChangeNotificationSubscription = _pubSub.Subscribe<DirectoryContentChangeNotification>(DirectoryContentChangeHandler);
        }

        private void UnsubscribeFromNotification() {
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
                Tag = documentDirectory,
                ImageIndex = FOLDER_BLUE_ICON_INDEX
            });
        }

        private void FilterListView(string query) {
            //documentDirectoryListView.Items.OfType<ListViewItem>().Each(_ => {
            //    if (_.Text.IndexOf(query, StringComparison.InvariantCultureIgnoreCase) > 0) {
            //    }
            //});
        }

        private int GetImageListIndex(DocumentEntity document) {
            var extension = Path.GetExtension(document.Path).ToLower();
            return document.Indexed
                ? extension == "docx" ? WORD_DOCX_BLUE_ICON_INDEX : WORD_DOC_BLUE_ICON_INDEX
                : extension == "doc" ? WORD_DOCX_GRAY_ICON_INDEX : WORD_DOC_GRAY_ICON_INDEX;
        }

        #endregion Private Methods
    }
}