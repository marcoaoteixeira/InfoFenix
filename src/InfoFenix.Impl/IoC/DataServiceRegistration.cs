using System.Data.SQLite;
using System.IO;
using Autofac;
using InfoFenix.Core;
using InfoFenix.Core.Data;
using InfoFenix.Core.IoC;

namespace InfoFenix.Impl.IoC {

    public sealed class DataServiceRegistration : ServiceRegistrationBase {

        #region Private Methods

        private DatabaseSettings GetDatabaseSettings(IComponentContext ctx) {
            var appSettings = ctx.Resolve<IAppSettings>();

            return new DatabaseSettings {
                ConnectionString = $"Data Source={Path.Combine(appSettings.ApplicationDataDirectoryPath, Common.DatabaseFileName)}; Version=3;",
                ProviderName = nameof(SQLiteFactory)
            };
        }

        #endregion Private Methods

        #region Public Override Methods

        public override void Register() {
            Builder
                .Register(GetDatabaseSettings)
                .AsSelf()
                .SingleInstance();
            Builder
                .RegisterType<DbProvider>()
                .As<IDbProvider>()
                .SingleInstance();
            Builder
                .RegisterType<Database>()
                .As<IDatabase>()
                .SingleInstance();
        }

        #endregion Public Override Methods
    }
}