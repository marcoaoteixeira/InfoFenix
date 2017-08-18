using Autofac;
using InfoFenix.Office;

namespace InfoFenix.IoC {

    public sealed class WordDocumentServiceRegistration : ServiceRegistrationBase {

        #region Public Override Methods

        public override void Register() {
            //Builder
            //    .RegisterType<SpireDocWordApplication>()
            //    .As<IWordApplication>()
            //    .SingleInstance();
            Builder
                .RegisterType<MicrosoftWordApplication>()
                .As<IWordApplication>()
                .SingleInstance();
        }

        #endregion Public Override Methods
    }
}