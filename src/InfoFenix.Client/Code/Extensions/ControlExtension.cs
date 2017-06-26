using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace InfoFenix.Client.Code {

    internal static class ControlExtension {

        #region Internal Static Methods

        internal static void SafeCall<TControl>(this TControl source, MethodInvoker action) where TControl : Control {
            if (source == null) { return; }
            if (source.IsDisposed) { return; }
            if (source.InvokeRequired) { source.Invoke(action); }
            else { action(); }
        }

        internal static TResult SafeInvoke<T, TResult>(this T source, Func<T, TResult> function) where T : ISynchronizeInvoke {
            if (source.InvokeRequired) {
                var result = source.BeginInvoke(function, new object[] { source });
                var end = source.EndInvoke(result);

                return (TResult)end;
            } else { return function(source); }
        }

        internal static void SafeInvoke<T>(this T source, Action<T> action) where T : ISynchronizeInvoke {
            if (source.InvokeRequired) { source.BeginInvoke(action, new object[] { source }); }
            else { action(source); }
        }


        #endregion Internal Static Methods
    }
}