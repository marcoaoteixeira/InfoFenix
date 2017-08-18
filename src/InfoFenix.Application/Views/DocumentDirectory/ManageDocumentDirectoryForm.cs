using System;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using InfoFenix.Application.Code;
using InfoFenix.Application.Views.Shared;
using InfoFenix;
using InfoFenix.Domains.Commands;
using InfoFenix.CQRS;
using InfoFenix.Domains.Queries;
using InfoFenix.Infrastructure;

namespace InfoFenix.Application.Views.DocumentDirectory {

    public partial class ManageDocumentDirectoryForm : LayoutForm {

        #region Private Constants

        private const string CONTEXT_MENU_TAG_EDIT = "EDIT";
        private const string CONTEXT_MENU_TAG_INDEX = "INDEX";
        private const string CONTEXT_MENU_TAG_CLEAN = "CLEAN";
        private const string CONTEXT_MENU_TAG_REMOVE = "REMOVE";

        private const string CHANGE_POSITION_TOP_TAG = "TOP";
        private const string CHANGE_POSITION_UP_TAG = "UP";
        private const string CHANGE_POSITION_DOWN_TAG = "DOWN";
        private const string CHANGE_POSITION_BOTTOM_TAG = "BOTTOM";

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

        private void EditDocumentDirectory(Entities.DocumentDirectory documentDirectory) {
            using (var dialog = _formManager.Get<DocumentDirectoryForm>()) {
                dialog.ViewModel = documentDirectory;
                if (dialog.ShowDialog() != DialogResult.OK) { return; }

                FillDocumentDirectoryDataGridView();
            }
        }

        private void IndexDocumentDirectory(Entities.DocumentDirectory documentDirectory) {
            ProgressViewer.Display(_cancellationTokenIssuer, actions: (token, progress) => {
                _mediator.CommandAsync(new IndexDocumentDirectoryCommand {
                    DocumentDirectoryID = documentDirectory.DocumentDirectoryID
                }, token, progress).Wait();
            });
        }

        private void CleanDocumentDirectory(Entities.DocumentDirectory documentDirectory) {
            var actions = new Action<CancellationToken, IProgress<ProgressInfo>>[] {
                (token, progress) => {
                    _mediator.CommandAsync(new SaveDocumentDirectoryDocumentsCommand {
                        DocumentDirectoryID = documentDirectory.DocumentDirectoryID
                    }, token, progress).Wait();
                },
                (token, progress) => {
                    _mediator.CommandAsync(new CleanDocumentDirectoryCommand {
                        DocumentDirectoryID = documentDirectory.DocumentDirectoryID
                    }, token, progress).Wait();
                }
            };
            ProgressViewer.Display(_cancellationTokenIssuer, actions: actions);
        }

        private void RemoveDocumentDirectory(Entities.DocumentDirectory documentDirectory) {
            var confirm = MessageBox.Show($"Deseja realmente remover o diretório de documentos \"{documentDirectory.Label}\"?"
                , "Remover Diretório de Documentos"
                , MessageBoxButtons.YesNo
                , MessageBoxIcon.Question);
            if (confirm == DialogResult.No) { return; }

            ProgressViewer.Display(_cancellationTokenIssuer, actions: (token, progress) => {
                _mediator.CommandAsync(new RemoveDocumentDirectoryCommand {
                    DocumentDirectoryID = documentDirectory.DocumentDirectoryID
                }, token, progress).Wait();
            });

            FillDocumentDirectoryDataGridView();
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
                var documentDirectory = row.DataBoundItem as Entities.DocumentDirectory;

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

        private void ChangePosition_Click(object sender, EventArgs e) {
            if (documentDirectoryDataGridView.SelectedRows.Count == 0) {
                MessageBox.Show("Selecione um diretório de documentos para mudar sua posição."
                    , "Selecionar"
                    , MessageBoxButtons.OK
                    , MessageBoxIcon.Information);
                return;
            }

            if (sender is Button button) {
                var documentDirectory = documentDirectoryDataGridView.SelectedRows[0].DataBoundItem as Entities.DocumentDirectory;
                switch (button.Tag.ToString().ToUpperInvariant()) {
                    case CHANGE_POSITION_TOP_TAG:
                        documentDirectory.Position = DocumentDirectoryForm.MIN_POSITION;
                        break;
                    case CHANGE_POSITION_UP_TAG:
                        documentDirectory.Position = documentDirectory.Position <= DocumentDirectoryForm.MIN_POSITION
                            ? DocumentDirectoryForm.MIN_POSITION
                            : documentDirectory.Position - 1;
                        break;
                    case CHANGE_POSITION_DOWN_TAG:
                        documentDirectory.Position = documentDirectory.Position >= DocumentDirectoryForm.MAX_POSITION
                            ? DocumentDirectoryForm.MAX_POSITION
                            : documentDirectory.Position + 1;
                        break;
                    case CHANGE_POSITION_BOTTOM_TAG:
                        documentDirectory.Position = DocumentDirectoryForm.MAX_POSITION;
                        break;
                }

                _mediator.Command(new SaveDocumentDirectoryCommand {
                    Code = documentDirectory.Code,
                    DocumentDirectoryID = documentDirectory.DocumentDirectoryID,
                    Label = documentDirectory.Label,
                    Path = documentDirectory.Path,
                    Position = documentDirectory.Position
                });

                FillDocumentDirectoryDataGridView();
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