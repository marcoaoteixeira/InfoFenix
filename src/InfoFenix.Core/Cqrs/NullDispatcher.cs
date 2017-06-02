﻿namespace InfoFenix.Core.Cqrs {
    public sealed class NullDispatcher : ICommandQueryDispatcher {

        #region Public Static Read-Only Fields

        public static readonly ICommandQueryDispatcher Instance = new NullDispatcher();

        #endregion Public Static Read-Only Fields

        #region Private Constructors

        private NullDispatcher() {
        }

        #endregion Private Constructors

        #region IDispatcher Members

        public void Command(ICommand command) {
        }

        public TResult Query<TResult>(IQuery<TResult> query) {
            return default(TResult);
        }

        #endregion IDispatcher Members
    }
}