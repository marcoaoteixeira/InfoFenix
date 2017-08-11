using System.Data.SQLite;
using System.IO;
using Autofac;
using InfoFenix.Core.Data;

namespace InfoFenix.Core.IoC {

    public sealed class DataServiceRegistration : ServiceRegistrationBase {

        #region Private Methods

        private DatabaseSettings GetDatabaseSettings(IComponentContext ctx) {
            return new DatabaseSettings {
                ConnectionString = $"Data Source={Path.Combine(AppSettings.Instance.ApplicationDataDirectoryPath, Common.DatabaseFileName)}; Version=3;",
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