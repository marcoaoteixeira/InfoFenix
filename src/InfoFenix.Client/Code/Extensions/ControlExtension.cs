using System.Threading;
using System.Windows.Forms;

namespace InfoFenix.Client.Code {

    internal static class ControlExtension {

        #region Internal Static Methods

        internal static void SafeCall<TControl>(this TControl source, MethodInvoker action) where TControl : Control {
            if (source == null) { return; }
            while (!source.Visible) { Thread.Sleep(50); }
            if (source.InvokeRequired) { source.Invoke(action); } else { action(); }
        }

        #endregion Internal Static Methods
    }
}