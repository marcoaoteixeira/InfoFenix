using System;

namespace InfoFenix.PubSub {
    public interface ISubscription<TMessage> {

        #region Methods

        Action<TMessage> CreateHandler();

        #endregion Methods
    }
}