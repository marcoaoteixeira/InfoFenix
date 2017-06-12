using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace InfoFenix.Core.PubSub {
    /// <summary>
    /// Default implementation of <see cref="IPublisherSubscriber"/>.
    /// </summary>
    public sealed class PublisherSubscriber : IPublisherSubscriber, IDisposable {

        #region Private Static Read-Only Fields

        private static readonly object SyncLock = new object();

        #endregion Private Static Read-Only Fields

        #region Private Read-Only Fields

        private readonly IDictionary<Type, IList> _subscriptions;

        #endregion Private Read-Only Fields

        #region Private Fields

        private bool _disposed;

        #endregion Private Fields

        #region Public Constructors

        public PublisherSubscriber() {
            _subscriptions = new Dictionary<Type, IList>();
        }

        #endregion Public Constructors

        #region Destructors

        /// <summary>
        /// Destructor.
        /// </summary>
        ~PublisherSubscriber() {
            Dispose(disposing: false);
        }

        #endregion Destructors

        #region Private Methods

        private void Dispose(bool disposing) {
            if (_disposed) { return; }
            if (disposing) {
                lock (SyncLock) {
                    //_subscriptions.Each(subscriptionList => {
                    //    subscriptionList.Value.Each(subscription => {
                    //        if (subscription is IDisposable disposable) {
                    //            disposable.Dispose();
                    //        }
                    //    });
                    //});
                    _subscriptions.Clear();
                }
            }
            _disposed = true;
        }

        #endregion Private Methods

        #region IPublisherSubscriber Members

        /// <inheritdoc />
        public ISubscription<TMessage> Subscribe<TMessage>(Action<TMessage> handler) {
            Prevent.ParameterNull(handler, nameof(handler));

            var messageType = typeof(TMessage);
            var action = new Subscription<TMessage>(handler, this);
            lock (SyncLock) {
                if (!_subscriptions.TryGetValue(messageType, out IList list)) {
                    _subscriptions.Add(messageType, new List<ISubscription<TMessage>> { action });
                } else {
                    _subscriptions[messageType].Add(action);
                }
            }
            return action;
        }

        /// <inheritdoc />
        public bool Unsubscribe<TMessage>(ISubscription<TMessage> subscription) {
            Prevent.ParameterNull(subscription, nameof(subscription));

            var messageType = typeof(TMessage);
            if (_subscriptions.ContainsKey(messageType)) {
                lock (SyncLock) {
                    _subscriptions[messageType].Remove(subscription);
                }
            }
            return true;
        }

        /// <inheritdoc />
        public void Publish<TMessage>(TMessage message) {
            Prevent.ParameterNull(message, nameof(message));

            var messageType = typeof(TMessage);
            if (_subscriptions.ContainsKey(messageType)) {
                foreach (var subscription in _subscriptions[messageType].OfType<ISubscription<TMessage>>()) {
                    var handler = subscription.CreateHandler();
                    if (handler != null) {
                        handler.Invoke(message);
                    }
                }
            }
        }

        #endregion IPublisherSubscriber Members

        #region IDisposable Members

        /// <inheritdoc />
        public void Dispose() {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable Members
    }
}