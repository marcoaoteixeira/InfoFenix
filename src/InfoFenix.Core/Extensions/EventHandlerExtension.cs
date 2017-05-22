using System;

namespace InfoFenix.Core {
    public static class EventHandlerExtension {

        #region Public Static Methods

        public static void SafeInvoke<TEventArgs>(this EventHandler<TEventArgs> evt, object sender, TEventArgs e) where TEventArgs : EventArgs {
            evt?.Invoke(sender, e);
        }

        #endregion Public Static Methods
    }
}