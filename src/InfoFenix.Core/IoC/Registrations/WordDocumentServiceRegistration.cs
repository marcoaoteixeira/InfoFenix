using Autofac;
using InfoFenix.Core.Office;

namespace InfoFenix.Core.IoC {

    public sealed class WordDocumentServiceRegistration : ServiceRegistrationBase {

        #region Public Override Methods

        public override void Register() {
            Builder
                .RegisterType<SpireDocWordDocumentService>()
                .As<IWordDocumentService>()
                .SingleInstance();
        }

        #endregion Public Override Methods
    }
}