using System;
using System.Linq;
using System.Threading;
using WordApplication = Microsoft.Office.Interop.Word.Application;
using WordDocument = Microsoft.Office.Interop.Word.Document;

namespace InfoFenix.Core.Office {
    public class MicrosoftWordApplication : IMicrosoftWordApplication, IDisposable {

        #region Private Constants

        private const string WORD_INSTANCE_TITLE = "VOW_GUARDIAN_WORD_INSTANCE";

        #endregion Private Constants

        #region Private Read-Only Fields

        private readonly IAppSettings _appSettings;

        #endregion Private Read-Only Fields

        #region Private Fields

        private WordApplication _application;
        private object Unknown = Type.Missing;
        private bool _disposed;

        #endregion Private Fields

        #region Public Constructors

        public MicrosoftWordApplication(IAppSettings appSettings) {
            Prevent.ParameterNull(appSettings, nameof(appSettings));

            _appSettings = appSettings;

            Initialize();
        }

        #endregion Public Constructors

        #region Destructor

        ~MicrosoftWordApplication() {
            Dispose(disposing: false);
        }

        #endregion Destructor

        #region Private Methods

        private void Initialize() {
            _application = new WordApplication();
            _application.Caption = WORD_INSTANCE_TITLE;
            _application.Application.Visible = true;
            var processID = int.MaxValue;
            while (processID == int.MaxValue) {
                processID = Common.GetProcessID(WORD_INSTANCE_TITLE);
                Thread.Sleep(5);
            }
            _application.Application.Visible = false;
            _appSettings.LastOfficeWordProcessID = processID;
            _appSettings.Save();
        }

        private void Dispose(bool disposing) {
            if (_disposed) { return; }
            if (disposing) { Quit(); }

            _application = null;
            _disposed = true;
        }

        #endregion Private Methods

        #region IDisposable Members

        public void Dispose() {
            Dispose(disposing: true);
            GC.SuppressFinalize(obj: this);
        }

        #endregion IDisposable Members

        #region MicrosoftWordApplication Members

        public IMicrosoftWordDocument Open(string filePath) {
            var currentFilePath = (object)filePath;
            var document = _application.Documents.Open(FileName: ref currentFilePath
                , ConfirmConversions: ref Unknown
                , ReadOnly: ref Unknown
                , AddToRecentFiles: ref Unknown
                , PasswordDocument: ref Unknown
                , PasswordTemplate: ref Unknown
                , Revert: ref Unknown
                , WritePasswordDocument: ref Unknown
                , WritePasswordTemplate: ref Unknown
                , Format: ref Unknown
                , Encoding: ref Unknown
                , Visible: ref Unknown
                , OpenAndRepair: ref Unknown
                , DocumentDirection: ref Unknown
                , NoEncodingDialog: ref Unknown
                , XMLTransform: ref Unknown);

            return new MicrosoftWordDocument(document);
        }

        public void Quit() {
            if (_application != null) {
                foreach (WordDocument document in _application.Documents) {
                    document.Close(SaveChanges: ref Unknown
                        , OriginalFormat: ref Unknown
                        , RouteDocument: ref Unknown);
                }

                _application.Quit(SaveChanges: ref Unknown
                    , OriginalFormat: ref Unknown
                    , RouteDocument: ref Unknown);
            }
        }

        #endregion MicrosoftWordApplication Members
    }
}