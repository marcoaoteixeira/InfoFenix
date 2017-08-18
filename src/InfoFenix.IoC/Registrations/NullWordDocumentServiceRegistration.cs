using Autofac;
using InfoFenix.Office;

namespace InfoFenix.IoC {

    public sealed class NullWordDocumentServiceRegistration : ServiceRegistrationBase {

        #region Public Override Methods

        public override void Register() {
            Builder
                .RegisterInstance(NullWordApplication.Instance)
                .As<IWordApplication>()
                .SingleInstance();
        }

        #endregion Public Override Methods
    }
}