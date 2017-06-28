using System.Windows.Forms;
using InfoFenix.Core.Logging;

namespace InfoFenix.Client.Views.Shared {

    public partial class WaitForForm : Form {

        #region Public Properties

        public ILogger Log { get; }
        public bool Silent { get; }

        #endregion Public Properties

        #region Public Constructors

        public WaitForForm(ILogger log = null, bool silent = false) {
            Log = log ?? NullLogger.Instance;
            Silent = silent;

            InitializeComponent();
        }

        #endregion Public Constructors
    }
}