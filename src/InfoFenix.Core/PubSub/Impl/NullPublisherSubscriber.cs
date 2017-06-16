using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfoFenix.Core.PubSub {

    public sealed class NullPublisherSubscriber : IPublisherSubscriber {

        #region Public Static Read-Only Fields

        public static readonly IPublisherSubscriber Instance = new NullPublisherSubscriber();

        #endregion Public Static Read-Only Fields

        #region Private Constructors

        private NullPublisherSubscriber() {
        }

        #endregion Private Constructors

        #region IPublisherSubscriber Members

        public Task PublishAsync<TMessage>(TMessage message, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.FromResult(0);
        }

        public ISubscription<TMessage> Subscribe<TMessage>(Action<TMessage> handler) {
            return NullSubscription<TMessage>.Instance;
        }

        public bool Unsubscribe<TMessage>(ISubscription<TMessage> subscription) {
            return true;
        }

        #endregion IPublisherSubscriber Members
    }
}