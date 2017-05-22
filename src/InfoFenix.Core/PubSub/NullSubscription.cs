using System;

namespace InfoFenix.Core.PubSub {

    public sealed class NullSubscription<TMessage> : ISubscription<TMessage> {

        #region Public Static Read-Only Fields

        public static readonly ISubscription<TMessage> Instance = new NullSubscription<TMessage>();

        #endregion Public Static Read-Only Fields

        #region Private Constructors

        private NullSubscription() {
        }

        #endregion Private Constructors

        #region Private Static Methods

        private static void DoNothing(TMessage message) {
        }

        #endregion Private Static Methods

        #region ISubscription<TMessage> Members

        public Action<TMessage> CreateHandler() {
            return DoNothing;
        }

        #endregion ISubscription<TMessage> Members
    }
}