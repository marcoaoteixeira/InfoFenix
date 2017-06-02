using Autofac;
using InfoFenix.Core.Services;

namespace InfoFenix.Core.IoC {

    public sealed class ServicesServiceRegistration : ServiceRegistrationBase {

        #region Public Override Methods

        public override void Register() {
            Builder
                .RegisterType<DocumentDirectoryService>()
                .As<IDocumentDirectoryService>()
                .SingleInstance();
        }

        #endregion Public Override Methods
    }
}