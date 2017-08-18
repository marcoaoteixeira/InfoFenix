using Autofac;
using InfoFenix.Services;

namespace InfoFenix.IoC {

    public sealed class ServicesServiceRegistration : ServiceRegistrationBase {

        #region Public Override Methods

        public override void Register() {
            Builder
                .RegisterType<ManagementService>()
                .As<IManagementService>()
                .SingleInstance();

            Builder
                .RegisterType<SearchService>()
                .As<ISearchService>()
                .SingleInstance();
        }

        #endregion Public Override Methods
    }
}