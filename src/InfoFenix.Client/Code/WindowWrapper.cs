using System;
using System.Windows.Forms;

namespace InfoFenix.Client.Code {

    public sealed class WindowWrapper : IWin32Window {

        #region Private Read-Only Fields

        private readonly IntPtr _handle;

        #endregion Private Read-Only Fields

        #region Private Constructors

        private WindowWrapper(IntPtr handle) {
            _handle = handle;
        }

        #endregion Private Constructors

        #region Public Static Methods

        public static IWin32Window Create(IntPtr handle) {
            return new WindowWrapper(handle);
        }

        #endregion Public Static Methods

        #region IWin32Window Members

        public IntPtr Handle => _handle;

        #endregion IWin32Window Members
    }
}