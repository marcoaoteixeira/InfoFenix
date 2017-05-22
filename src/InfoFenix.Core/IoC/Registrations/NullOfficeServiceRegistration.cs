using Autofac;
using InfoFenix.Core.IoC;
using InfoFenix.Core.Office;

namespace InfoFenix.Core.IoC {
    public sealed class NullOfficeServiceRegistration : ServiceRegistrationBase {

        #region Public Override Methods

        public override void Register() {
            Builder
                .RegisterInstance(NullMicrosoftWordApplication.Instance)
                .As<IMicrosoftWordApplication>()
                .SingleInstance();
        }

        #endregion Public Override Methods
    }
}