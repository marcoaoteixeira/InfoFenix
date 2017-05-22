using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SQLite;

namespace InfoFenix.Core.Data {
    /// <summary>
    /// Default implementation of <see cref="IDbProvider"/>
    /// </summary>
    public class DbProvider : IDbProvider {

        #region IDbProvider Members

        /// <inheritdoc />
        public DbProviderFactory GetFactory(Type providerFactoryType) {
            Prevent.ParameterNull(providerFactoryType, nameof(providerFactoryType));

            return GetFactory(providerFactoryType.Name);
        }

        /// <inheritdoc />
        public DbProviderFactory GetFactory(string providerFactoryName) {
            Prevent.ParameterNullOrWhiteSpace(providerFactoryName, nameof(providerFactoryName));

            switch (providerFactoryName) {
                case nameof(SqlClientFactory):
                    return SqlClientFactory.Instance;

                case nameof(SQLiteFactory):
                    return SQLiteFactory.Instance;

                default:
                    throw new InvalidOperationException("DB Provider Factory not implemented.");
            }
        }

        #endregion IDbProvider Members
    }
}