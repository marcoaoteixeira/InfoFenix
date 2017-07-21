using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using InfoFenix.Client.Code;
using InfoFenix.Client.Code.Controls;
using InfoFenix.Client.Views.Shared;
using InfoFenix.Core;
using InfoFenix.Core.Commands;
using InfoFenix.Core.Cqrs;
using InfoFenix.Core.Dto;
using InfoFenix.Core.Queries;
using Resource = InfoFenix.Client.Properties.Resources;

namespace InfoFenix.Client.Views.DocumentDirectory {

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

        private const string CANCELLATION_TOKEN_FILL_LISTVIEW = "CANCELLATION::TOKEN::FILL::LISTVIEW";

        #endregion Private Constants

        #region Private Read-Only Fields

        private readonly CancellationTokenIssuer _cancellationTokenIssuer;
        private readonly IFormManager _formManager;
        private readonly IMediator _mediator;

        #endregion Private Read-Only Fields

        #region Private Fields

        private DocumentDirectoryDto _currentDocumentDirectory;
        private CancellationToken _cancellationTokenFillListView;

        #endregion Private Fields

        #region Public Constructors

        public ManageDocumentDirectoryForm(CancellationTokenIssuer cancellationTokenIssuer, IFormManager formManager, IMediator mediator) {
            Prevent.ParameterNull(cancellationTokenIssuer, nameof(cancellationTokenIssuer));
            Prevent.ParameterNull(formManager, nameof(formManager));
            Prevent.ParameterNull(mediator, nameof(mediator));

            _cancellationTokenIssuer = cancellationTokenIssuer;
            _formManager = formManager;
            _mediator = mediator;

            InitializeComponent();
        }

        #endregion Public Constructors

        #region Private Methods

        private void Initialize() {
            documentDirectoryTreeView.ImageList = manageDocumentDirectoryImageList;
            documentListView.LargeImageList = manageDocumentDirectoryImageList;
            documentListView.SmallImageList = manageDocumentDirectoryImageList;

            WaitFor.Execution(() => {
                _mediator
                   .Query(new ListDocumentDirectoriesQuery { RequireDocuments = false })
                   .OrderBy(_ => _.DocumentDirectoryID)
                   .Each(InsertTreeViewEntry);
            }, silent: true);
        }

        private void InsertTreeViewEntry(DocumentDirectoryDto documentDirectory) {
            var node = documentDirectoryTreeView.SafeInvoke(_ => _.Nodes.Find(key: documentDirectory.Code, searchAllChildren: false).SingleOrDefault());

            // Insert or update node
            if (node == null) {
                node = new TreeNode {
                    Name = documentDirectory.Code,
                    Text = documentDirectory.Label,
                    Tag = documentDirectory,
                    ImageIndex = FOLDER_BLUE_ICON_INDEX
                };
            } else {
                node.Text = documentDirectory.Label;
                node.Tag = documentDirectory;
            }

            documentDirectoryTreeView.SafeInvoke(_ => {
                if (!_.Nodes.Contains(node)) { _.Nodes.Add(node); }
            });
        }

        private Task FillListViewAsync(DocumentDirectoryDto documentDirectory, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => {
                if (_currentDocumentDirectory != documentDirectory) {
                    _currentDocumentDirectory = documentDirectory;
                    this.SafeInvoke(_ => _.Cursor = Cursors.WaitCursor);
                    BindListView(documentDirectory.Documents);
                    this.SafeInvoke(_ => _.Cursor = Cursors.Default);
                }
            });
        }

        private void BindListView(IEnumerable<DocumentDto> documents) {
            documentListView.SafeInvoke(_ => _.Clear());
            directoryInformationLabel.SafeInvoke(_ => _.Text = string.Format(Resource.TotalDocumentsMessage, documents.Count()));

            var itemList = new List<ListViewItem>();
            foreach (var document in documents.OrderBy(_ => _.Code)) {
                var name = document.FileName;
                var item = documentListView.SafeInvoke(_ => _.Items.Find(key: name, searchAllSubItems: false).SingleOrDefault());
                var toolTip = document.Indexed ? Resource.Indexed : Resource.NonIndexed;
                if (item == null) {
                    item = new ListViewItem {
                        Text = name,
                        ImageIndex = GetImageListIndex(document),
                        Name = name,
                        Tag = document,
                        ToolTipText = toolTip
                    };
                } else {
                    item.ImageIndex = GetImageListIndex(document);
                    item.ToolTipText = toolTip;
                    item.Tag = document;
                }
                itemList.Add(item);
            }
            documentListView.SafeInvoke(_ => _.Items.AddRange(itemList.ToArray()));
        }

        private void FilterListViewByName(string term) {
            this.SafeInvoke(_ => _.Cursor = Cursors.WaitCursor);

            var documents = !string.IsNullOrWhiteSpace(term)
                ? _currentDocumentDirectory.Documents.Where(_ => (_.FileName.IndexOf(term, StringComparison.OrdinalIgnoreCase) > 0))
                : _currentDocumentDirectory.Documents;

            BindListView(documents);

            this.SafeInvoke(_ => _.Cursor = Cursors.Default);
        }

        private void InsertListViewEntry(DocumentDto document) {
            var name = document.FileName;
            var item = documentListView.SafeInvoke(_ => _.Items.Find(key: name, searchAllSubItems: false).SingleOrDefault());
            var toolTip = document.Indexed ? Resource.Indexed : Resource.NonIndexed;
            if (item == null) {
                item = new ListViewItem {
                    Text = name,
                    ImageIndex = GetImageListIndex(document),
                    Name = name,
                    Tag = document,
                    ToolTipText = toolTip
                };
            } else {
                item.ImageIndex = GetImageListIndex(document);
                item.ToolTipText = toolTip;
                item.Tag = document;
            }
            documentListView.SafeInvoke(_ => _.Items.Add(item));
        }

        private int GetImageListIndex(DocumentDto document) {
            var extension = Path.GetExtension(document.Path).ToLower();
            return document.Indexed
                ? extension == "docx" ? WORD_DOCX_BLUE_ICON_INDEX : WORD_DOC_BLUE_ICON_INDEX
                : extension == "doc" ? WORD_DOCX_GRAY_ICON_INDEX : WORD_DOC_GRAY_ICON_INDEX;
        }

        private void EditDocumentDirectory(DocumentDirectoryDto documentDirectory) {
            using (var dialog = _formManager.Get<DocumentDirectoryForm>()) {
                dialog.ViewModel = documentDirectory;
                if (dialog.ShowDialog() != DialogResult.OK) { return; }

                InsertTreeViewEntry(dialog.ViewModel);
            }
        }

        private void IndexDocumentDirectory(DocumentDirectoryDto documentDirectory) {
            ProgressViewer.Display(_cancellationTokenIssuer, actions: (token, progress) => {
                var documents = _mediator.Query(new ListDocumentsByDocumentDirectoryQuery {
                    DocumentDirectoryID = documentDirectory.DocumentDirectoryID
                });

                _mediator.CommandAsync(new IndexDocumentCollectionCommand {
                    BatchSize = 128,
                    Documents = documents.Where(_ => !_.Indexed).Select(DocumentIndexDto.Map).ToList(),
                    IndexName = documentDirectory.Code
                }, token, progress);
            });
        }

        private void StartWatchDocumentDirectory(DocumentDirectoryDto documentDirectory) {
            ProgressViewer.Display(_cancellationTokenIssuer, actions: (token, progress) => {
                _mediator.CommandAsync(new StartWatchDocumentDirectoryCommand {
                    DocumentDirectory = documentDirectory
                }, token, progress);
            });
        }

        private void StopWatchDocumentDirectory(DocumentDirectoryDto documentDirectory) {
            ProgressViewer.Display(_cancellationTokenIssuer, actions: (token, progress) => {
                _mediator.CommandAsync(new StopWatchDocumentDirectoryCommand {
                    DocumentDirectory = documentDirectory
                }, token, progress);
            });
        }

        private void RemoveDocumentDirectory(DocumentDirectoryDto documentDirectory) {
            ProgressViewer.Display(_cancellationTokenIssuer, actions: (token, progress) => {
                _mediator.CommandAsync(new RemoveDocumentDirectoryCommand {
                    DocumentDirectory = documentDirectory
                }, token, progress);
            });
        }

        private void IndexDocument(DocumentDto document) {
        }

        private void RemoveDocument(DocumentDto document) {
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
                var documentDirectory = node.Tag as DocumentDirectoryDto;

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
                        documentDirectoryTreeView.Nodes.Remove(node);
                        documentListView.Clear();
                        break;
                }
            }
        }

        private void documentToolStripMenuItem_Click(object sender, EventArgs e) {
            if (sender is ToolStripMenuItem menu) {
                var item = documentListView.SelectedItems.OfType<ListViewItem>().SingleOrDefault();
                if (item == null) { return; }
                var document = item.Tag as DocumentDto;

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
            if (e.Node.Tag is DocumentDirectoryDto documentDirectory) {
                if (documentDirectory == _currentDocumentDirectory) { return; }

                if (_cancellationTokenFillListView != null && !_cancellationTokenFillListView.IsCancellationRequested) {
                    _cancellationTokenIssuer.Cancel(CANCELLATION_TOKEN_FILL_LISTVIEW);
                }

                _cancellationTokenFillListView = _cancellationTokenIssuer.Get(CANCELLATION_TOKEN_FILL_LISTVIEW);

                var documents = _mediator.Query(new ListDocumentsByDocumentDirectoryQuery {
                    DocumentDirectoryID = documentDirectory.DocumentDirectoryID
                });
                documentDirectory.Documents = new List<DocumentDto>(documents);

                FillListViewAsync(documentDirectory, _cancellationTokenFillListView);
            }
        }

        private void filterListViewTextBox_DelayedTextChanged(object sender, EventArgs e) {
            if (sender is PowerTextBox textBox) {
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