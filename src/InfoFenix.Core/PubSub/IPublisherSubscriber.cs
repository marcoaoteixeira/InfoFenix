using System;

namespace InfoFenix.PubSub {

    /// <summary>
    /// Defines methods to implement a topic based publisher/subscriber.
    /// </summary>
	public interface IPublisherSubscriber {

        #region Methods

        /// <summary>
        /// Subscribes a handler to a topic.
        /// </summary>
        /// <typeparam name="TMessage">Type of the message.</typeparam>
        /// <param name="handler">The handler.</param>
        /// <returns>The instance of the</returns>
        ISubscription<TMessage> Subscribe<TMessage>(Action<TMessage> handler);

        /// <summary>
        /// Unsubscribes the handler of a topic.
        /// </summary>
        /// <typeparam name="TMessage">Type of the message.</typeparam>
        /// <param name="subscription">The subscription.</param>
        /// <returns><c>true</c> if can unsubscribe; otherwise, <c>false</c>.</returns>
		bool Unsubscribe<TMessage>(ISubscription<TMessage> subscription);

        /// <summary>
        /// Publishes a data notification for a specific topic.
        /// </summary>
        /// <typeparam name="TMessage">Type of the message.</typeparam>
        /// <param name="message">The message.</param>
		void Publish<TMessage>(TMessage message);

        #endregion Methods
    }
}