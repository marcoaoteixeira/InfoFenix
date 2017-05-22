using Autofac;
using InfoFenix.Core.IoC;
using InfoFenix.Core.PubSub;

namespace InfoFenix.Impl.IoC {

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