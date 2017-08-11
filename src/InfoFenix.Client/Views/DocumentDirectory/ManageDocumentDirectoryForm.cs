using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using InfoFenix.Client.Code;
using InfoFenix.Client.Views.Shared;
using InfoFenix.Core;
using InfoFenix.Core.Commands;
using InfoFenix.Core.CQRS;
using InfoFenix.Core.Queries;

namespace InfoFenix.Client.Views.DocumentDirectory {

    public partial class ManageDocumentDirectoryForm : LayoutForm {

        #region Private Constants

        private const string CONTEXT_MENU_TAG_EDIT = "EDIT";
        private const string CONTEXT_MENU_TAG_INDEX = "INDEX";
        private const string CONTEXT_MENU_TAG_CLEAN = "CLEAN";
        private const string CONTEXT_MENU_TAG_REMOVE = "REMOVE";

        #endregion Private Constants

        #region Private Read-Only Fields

        private readonly CancellationTokenIssuer _cancellationTokenIssuer;
        private readonly IFormManager _formManager;
        private readonly IMediator _mediator;

        #endregion Private Read-Only Fields

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
            documentDirectoryDataGridView.AutoGenerateColumns = false;

            FillDocumentDirectoryDataGridView();
        }

        private void FillDocumentDirectoryDataGridView() {
            documentDirectoryDataGridView.DataSource = null;

            var documentDirectories = _mediator
                .Query(new ListDocumentDirectoriesQuery())
                .OrderBy(_ => _.Position);

            documentDirectoryDataGridView.DataSource = documentDirectories.ToArray();
        }

        private void EditDocumentDirectory(Core.Entities.DocumentDirectory documentDirectory) {
            using (var dialog = _formManager.Get<DocumentDirectoryForm>()) {
                dialog.ViewModel = documentDirectory;
                if (dialog.ShowDialog() != DialogResult.OK) { return; }

                FillDocumentDirectoryDataGridView();
            }
        }

        private void IndexDocumentDirectory(Core.Entities.DocumentDirectory documentDirectory) {
            ProgressViewer.Display(_cancellationTokenIssuer, actions: (token, progress) => {
                _mediator.CommandAsync(new IndexDocumentDirectoryCommand {
                    DocumentDirectoryID = documentDirectory.DocumentDirectoryID,
                    BatchSize = 128
                }, token, progress);
            });
        }

        private void CleanDocumentDirectory(Core.Entities.DocumentDirectory documentDirectory) {
            ProgressViewer.Display(_cancellationTokenIssuer, actions: (token, progress) => {
                _mediator.CommandAsync(new CleanDocumentDirectoryCommand {
                    DocumentDirectoryID = documentDirectory.DocumentDirectoryID
                }, token, progress);
            });
        }

        private void RemoveDocumentDirectory(Core.Entities.DocumentDirectory documentDirectory) {
            ProgressViewer.Display(_cancellationTokenIssuer, actions: (token, progress) => {
                _mediator.CommandAsync(new RemoveDocumentDirectoryCommand {
                    DocumentDirectoryID = documentDirectory.DocumentDirectoryID
                }, token, progress);
            });
        }

        #endregion Private Methods

        #region Event Handlers

        private void ManageDocumentDirectoryForm_Load(object sender, EventArgs e) {
            Initialize();
        }

        private void documentDirectoryDataGridView_MouseClick(object sender, MouseEventArgs e) {
            if (e.Button != MouseButtons.Right) { return; }

            if (sender is DataGridView dataGridView) {
                var currentMouseOverRow = dataGridView.HitTest(e.X, e.Y).RowIndex;
                if (currentMouseOverRow > -1) {
                    dataGridView.Rows[currentMouseOverRow].Selected = true;
                    documentDirectoryContextMenuStrip.Show(dataGridView, new Point(e.X, e.Y));
                }
            }
        }

        private void documentDirectoryToolStripMenuItem_Click(object sender, EventArgs e) {
            if (sender is ToolStripMenuItem menu && documentDirectoryDataGridView.SelectedRows.Count > 0) {
                var row = documentDirectoryDataGridView.SelectedRows[0];
                if (row == null) { return; }
                var documentDirectory = row.DataBoundItem as Core.Entities.DocumentDirectory;

                switch ((string)menu.Tag) {
                    case CONTEXT_MENU_TAG_EDIT:
                        EditDocumentDirectory(documentDirectory);
                        break;

                    case CONTEXT_MENU_TAG_INDEX:
                        IndexDocumentDirectory(documentDirectory);
                        break;

                    case CONTEXT_MENU_TAG_CLEAN:
                        CleanDocumentDirectory(documentDirectory);
                        break;

                    case CONTEXT_MENU_TAG_REMOVE:
                        RemoveDocumentDirectory(documentDirectory);
                        break;
                }
            }
        }

        private void newDocumentDirectoryButton_Click(object sender, EventArgs e) {
            using (var dialog = _formManager.Get<DocumentDirectoryForm>()) {
                if (dialog.ShowDialog() != DialogResult.OK) { return; }

                FillDocumentDirectoryDataGridView();
            }
        }

        private void closeButton_Click(object sender, EventArgs e) {
            Close();
        }

        #endregion Event Handlers
    }
}