using Autofac;
using InfoFenix.Core.IoC;
using InfoFenix.Core.Office;

namespace InfoFenix.Core.IoC {
    public sealed class OfficeServiceRegistration : ServiceRegistrationBase {

        #region Public Override Methods

        public override void Register() {
            Builder
                .RegisterType<MicrosoftWordApplication>()
                .As<IMicrosoftWordApplication>()
                .SingleInstance();
        }

        #endregion Public Override Methods
    }
}