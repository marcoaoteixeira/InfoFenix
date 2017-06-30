using System.IO;
using Autofac;
using InfoFenix.Core.IoC;
using InfoFenix.Core.Search;

namespace InfoFenix.Core.IoC {

    public sealed class SearchServiceRegistration : ServiceRegistrationBase {

        #region Private Methods

        private LuceneSettings GetLuceneSettings(IComponentContext ctx) {
            var appSettings = ctx.Resolve<IAppSettings>();

            return new LuceneSettings {
                IndexStorageDirectoryPath = Path.Combine(appSettings.ApplicationDataDirectoryPath, Common.IndexStorageDirectoryName)
            };
        }

        #endregion Private Methods

        #region Public Override Methods

        public override void Register() {
            Builder
                .Register(GetLuceneSettings)
                .SingleInstance();
            Builder
                .RegisterType<IndexProvider>()
                .As<IIndexProvider>()
                .SingleInstance();
            Builder
                .RegisterType<AnalyzerProvider>()
                .As<IAnalyzerProvider>()
                .SingleInstance();
            Builder
                .RegisterType<AnalyzerSelector>()
                .As<IAnalyzerSelector>()
                .SingleInstance();
            Builder
                .RegisterType<SearchBuilder>()
                .As<ISearchBuilder>()
                .InstancePerDependency();
        }

        #endregion Public Override Methods
    }
}