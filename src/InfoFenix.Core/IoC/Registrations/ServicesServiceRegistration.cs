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

            Builder
                .RegisterType<SearchIndexService>()
                .As<ISearchIndexService>()
                .SingleInstance();

            Builder
                .RegisterType<DocumentService>()
                .As<IDocumentService>()
                .SingleInstance();
        }

        #endregion Public Override Methods
    }
}