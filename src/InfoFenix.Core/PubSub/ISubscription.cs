using System;

namespace InfoFenix.Core.PubSub {

    public interface ISubscription<TMessage> {

        #region Methods

        Action<TMessage> CreateHandler();

        #endregion Methods
    }
}