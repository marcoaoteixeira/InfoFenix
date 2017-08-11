using Autofac;
using InfoFenix.Core.PubSub;

namespace InfoFenix.Core.IoC {

    public sealed class PubSubServiceRegistration : ServiceRegistrationBase {

        #region Public Override Methods

        public override void Register() {
            Builder
                .RegisterType<PublisherSubscriber>()
                .As<IPublisherSubscriber>()
                .SingleInstance();
        }

        #endregion Public Override Methods
    }
}