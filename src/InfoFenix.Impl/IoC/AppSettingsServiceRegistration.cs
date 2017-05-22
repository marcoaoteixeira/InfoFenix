using Autofac;
using InfoFenix.Core;
using InfoFenix.Core.IoC;

namespace InfoFenix.Impl.IoC {

    public sealed class AppSettingsServiceRegistration : ServiceRegistrationBase {

        #region Public Override Methods

        public override void Register() {
            Builder
                .RegisterInstance(AppSettingsManager.Get())
                .As<IAppSettings>()
                .SingleInstance();
        }

        #endregion Public Override Methods
    }
}