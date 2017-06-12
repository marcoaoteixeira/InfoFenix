using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InfoFenix.Client.Views {

    public partial class WaitForm : Form {

        #region Public Constructors

        public WaitForm() {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Public Static Methods

        public static void WaitFor(Action action) {
            using (var form = new WaitForm()) {
                Task
                    .Run(action)
                    .ContinueWith(Continuation, form);

                form.ShowDialog();
            }
        }

        #endregion Public Static Methods

        #region Private Static Methods

        private static void Continuation(Task continuation, object state) {
            if (continuation.Exception != null) { MessageBox.Show($"Erro: {continuation.Exception.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            else { MessageBox.Show($"Ação concluída com sucesso.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information); }

            (state as WaitForm).DialogResult = DialogResult.OK;
        }

        #endregion Private Static Methods
    }
}