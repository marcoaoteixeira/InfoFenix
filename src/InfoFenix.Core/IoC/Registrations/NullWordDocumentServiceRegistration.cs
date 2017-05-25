using Autofac;
using InfoFenix.Core.Office;

namespace InfoFenix.Core.IoC {

    public sealed class NullWordDocumentServiceRegistration : ServiceRegistrationBase {

        #region Public Override Methods

        public override void Register() {
            Builder
                .RegisterInstance(NullWordDocumentService.Instance)
                .As<IWordDocumentService>()
                .SingleInstance();
        }

        #endregion Public Override Methods
    }
}