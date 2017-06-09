using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using InfoFenix.Client.Code;
using InfoFenix.Client.Views.Shared;
using InfoFenix.Core;
using InfoFenix.Core.Entities;
using InfoFenix.Core.Services;

namespace InfoFenix.Client.Views {

    public partial class ManageDocumentDirectoryForm : LayoutForm {

        #region Private Constants

        private const int FOLDER_BLUE_ICON_INDEX = 0;
        private const int WORD_DOC_BLUE_ICON_INDEX = 1;
        private const int WORD_DOC_GRAY_ICON_INDEX = 2;
        private const int WORD_DOCX_BLUE_ICON_INDEX = 3;
        private const int WORD_DOCX_GRAY_ICON_INDEX = 4;

        private const string CONTEXT_MENU_TAG_EDIT = "EDIT";
        private const string CONTEXT_MENU_TAG_INDEX = "INDEX";
        private const string CONTEXT_MENU_TAG_START_WATCH = "START_WATCH";
        private const string CONTEXT_MENU_TAG_STOP_WATCH = "STOP_WATCH";
        private const string CONTEXT_MENU_TAG_REMOVE = "REMOVE";

        #endregion Private Constants

        #region Private Read-Only Fields

        private readonly IDocumentDirectoryService _documentDirectoryService;
        private readonly IFormManager _formManager;
        private readonly ProgressiveTaskExecutor _progressiveTaskExecutor;

        #endregion Private Read-Only Fields

        #region Private Fields

        private int _currentDocumentDirectory;
        private IEnumerable<DocumentEntity> _currentDocuments = Enumerable.Empty<DocumentEntity>();

        #endregion Private Fields

        #region Private Properties

        private IEnumerable<DocumentEntity> _currentDocumentDirectoryItems;

        private IEnumerable<DocumentEntity> CurrentDocumentDirectoryItems {
            get { return _currentDocumentDirectoryItems ?? Enumerable.Empty<DocumentEntity>(); }
            set { _currentDocumentDirectoryItems = value ?? Enumerable.Empty<DocumentEntity>(); }
        }

        #endregion Private Properties

        #region Public Constructors

        public ManageDocumentDirectoryForm(IDocumentDirectoryService documentDirectoryService, IFormManager formManager, ProgressiveTaskExecutor progressiveTaskExecutor) {
            Prevent.ParameterNull(documentDirectoryService, nameof(documentDirectoryService));
            Prevent.ParameterNull(formManager, nameof(formManager));
            Prevent.ParameterNull(progressiveTaskExecutor, nameof(progressiveTaskExecutor));

            _documentDirectoryService = documentDirectoryService;
            _formManager = formManager;
            _progressiveTaskExecutor = progressiveTaskExecutor;

            InitializeComponent();
        }

        #endregion Public Constructors

        #region Private Methods

        private void Initialize() {
            documentDirectoryTreeView.ImageList = manageDocumentDirectoryImageList;
            documentListView.LargeImageList = manageDocumentDirectoryImageList;
            documentListView.SmallImageList = manageDocumentDirectoryImageList;

            _documentDirectoryService.List().Each(InsertTreeViewEntry);
        }

        private void InsertTreeViewEntry(DocumentDirectoryEntity documentDirectory) {
            var currentNode = documentDirectoryTreeView.Nodes.Find(key: documentDirectory.Code, searchAllChildren: false).SingleOrDefault();

            // Insert or update node
            if (currentNode == null) {
                currentNode = new TreeNode {
                    Name = documentDirectory.Code,
                    Text = documentDirectory.Label,
                    Tag = documentDirectory,
                    ImageIndex = FOLDER_BLUE_ICON_INDEX
                };
            } else {
                currentNode.Text = documentDirectory.Label;
                currentNode.Tag = documentDirectory;
            }

            var index = currentNode.Index == 0
                ? documentDirectoryTreeView.Nodes.Count > 0
                    ? documentDirectoryTreeView.Nodes.Count - 1
                    : 0
                : currentNode.Index;
            documentDirectoryTreeView.Nodes.Insert(index, currentNode);
        }

        private void FillListView(DocumentDirectoryEntity documentDirectory) {
            if (_currentDocumentDirectory == documentDirectory.ID) { return; }

            _currentDocumentDirectory = documentDirectory.ID;
            _currentDocuments = _documentDirectoryService.GetDocuments(documentDirectory.ID);

            BindListView(_currentDocuments);
        }

        private void BindListView(IEnumerable<DocumentEntity> documents) {
            documentListView.Clear();

            foreach (var document in documents) {
                InsertListViewEntry(document);
            }

            directoryInformationLabel.Text = $"Total de documentos: {documents.Count()}";
        }

        private void FilterListViewByName(string term) {
            Cursor = Cursors.WaitCursor;

            var documents = !string.IsNullOrWhiteSpace(term)
                ? _currentDocuments.Where(_ => (_.FileName.IndexOf(term, StringComparison.OrdinalIgnoreCase) > 0))
                : _currentDocuments;

            BindListView(documents);

            Cursor = Cursors.Default;
        }

        private void InsertListViewEntry(DocumentEntity document) {
            var name = Path.GetFileNameWithoutExtension(document.FileName);
            var currentItem = documentListView.Items.Find(key: name, searchAllSubItems: false).SingleOrDefault();
            var toolTip = document.Indexed ? "Indexado" : "Não indexado";
            if (currentItem == null) {
                currentItem = new ListViewItem {
                    Text = name,
                    ImageIndex = GetImageListIndex(document),
                    Name = name,
                    Tag = document,
                    ToolTipText = toolTip
                };
            } else {
                currentItem.ImageIndex = GetImageListIndex(document);
                currentItem.ToolTipText = toolTip;
                currentItem.Tag = document;
            }

            var index = currentItem.Index == -1
                ? documentListView.Items.Count > 0
                    ? documentListView.Items.Count - 1
                    : 0
                : currentItem.Index;
            documentListView.Items.Insert(index, currentItem);
        }

        private int GetImageListIndex(DocumentEntity document) {
            var extension = Path.GetExtension(document.Path).ToLower();
            return document.Indexed
                ? extension == "docx" ? WORD_DOCX_BLUE_ICON_INDEX : WORD_DOC_BLUE_ICON_INDEX
                : extension == "doc" ? WORD_DOCX_GRAY_ICON_INDEX : WORD_DOC_GRAY_ICON_INDEX;
        }

        private void EditDocumentDirectory(DocumentDirectoryEntity documentDirectory) {
            using (var dialog = _formManager.Get<DocumentDirectoryForm>()) {
                dialog.ViewModel = documentDirectory;
                if (dialog.ShowDialog() != DialogResult.OK) { return; }

                InsertTreeViewEntry(dialog.ViewModel);
            }
        }

        private void IndexDocumentDirectory(DocumentDirectoryEntity documentDirectory) {
            _progressiveTaskExecutor.Execute((cancellationToken) => _documentDirectoryService.IndexAsync(documentDirectory.ID, cancellationToken));
        }

        private void StartWatchDocumentDirectory(DocumentDirectoryEntity documentDirectory) {
            _progressiveTaskExecutor.Execute(() => _documentDirectoryService.StartWatchForModification(documentDirectory.ID));
        }

        private void StopWatchDocumentDirectory(DocumentDirectoryEntity documentDirectory) {
            _progressiveTaskExecutor.Execute(() => _documentDirectoryService.StopWatchForModification(documentDirectory.ID));
        }

        private void RemoveDocumentDirectory(DocumentDirectoryEntity documentDirectory) {
            _progressiveTaskExecutor.Execute(() => _documentDirectoryService.Remove(documentDirectory.ID));
        }

        private void IndexDocument(DocumentEntity document) {
        }

        private void RemoveDocument(DocumentEntity document) {
        }

        #endregion Private Methods

        #region Event Handlers

        private void ManageDocumentDirectoryForm_Load(object sender, EventArgs e) {
            Initialize();
        }

        private void createDocumentDirectoryButton_Click(object sender, EventArgs e) {
            var button = sender as Button;
            if (button == null) { return; }

            using (var dialog = _formManager.Get<DocumentDirectoryForm>()) {
                if (dialog.ShowDialog() != DialogResult.OK) { return; }

                InsertTreeViewEntry(dialog.ViewModel);
            }
        }

        private void directoryTreeView_MouseUp(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Right) {
                documentDirectoryTreeView.SelectedNode = documentDirectoryTreeView.GetNodeAt(e.X, e.Y);
                if (documentDirectoryTreeView.SelectedNode != null) {
                    documentDirectoryContextMenuStrip.Show(documentDirectoryTreeView, e.Location);
                }
            }
        }

        private void documentDirectoryToolStripMenuItem_Click(object sender, EventArgs e) {
            if (sender is ToolStripMenuItem menu) {
                var node = documentDirectoryTreeView.SelectedNode;
                if (node == null) { return; }
                var documentDirectory = node.Tag as DocumentDirectoryEntity;

                switch ((string)menu.Tag) {
                    case CONTEXT_MENU_TAG_EDIT:
                        EditDocumentDirectory(documentDirectory);
                        break;

                    case CONTEXT_MENU_TAG_INDEX:
                        IndexDocumentDirectory(documentDirectory);
                        break;

                    case CONTEXT_MENU_TAG_START_WATCH:
                        StartWatchDocumentDirectory(documentDirectory);
                        break;

                    case CONTEXT_MENU_TAG_STOP_WATCH:
                        StopWatchDocumentDirectory(documentDirectory);
                        break;

                    case CONTEXT_MENU_TAG_REMOVE:
                        RemoveDocumentDirectory(documentDirectory);
                        break;
                }
            }
        }

        private void documentToolStripMenuItem_Click(object sender, EventArgs e) {
            if (sender is ToolStripMenuItem menu) {
                var item = documentListView.SelectedItems.OfType<ListViewItem>().SingleOrDefault();
                if (item == null) { return; }
                var document = item.Tag as DocumentEntity;

                switch ((string)menu.Tag) {
                    case CONTEXT_MENU_TAG_INDEX:
                        IndexDocument(document);
                        break;

                    case CONTEXT_MENU_TAG_REMOVE:
                        RemoveDocument(document);
                        break;
                }
            }
        }

        private void documentDirectoryTreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e) {
            if (e.Node.Tag is DocumentDirectoryEntity documentDirectory) {
                FillListView(documentDirectory);
            }
        }

        private void filterListViewTextBox_KeyUp(object sender, KeyEventArgs e) {
            if (sender is TextBox textBox) {
                FilterListViewByName(textBox.Text);
            }
        }

        private void documentListView_MouseDown(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Right) {
                var listViewItem = documentListView.GetItemAt(e.X, e.Y);
                if (listViewItem != null) {
                    documentContextMenuStrip.Show(documentListView, e.Location);
                }
            }
        }

        private void closeButton_Click(object sender, EventArgs e) {
            Close();
        }

        #endregion Event Handlers
    }
}