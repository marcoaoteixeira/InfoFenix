using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace InfoFenix.Core.Data {
    /// <summary>
    /// Default implementation of <see cref="IDatabase"/>.
    /// </summary>
    public sealed class Database : IDatabase, IDisposable {

        #region Private Read-Only Fields

        private readonly IDbProvider _dbProvider;
        private readonly DatabaseSettings _databaseSettings;

        #endregion Private Read-Only Fields

        #region Private Fields

        private DbProviderFactory _factory;
        private DbConnection _connection;
        private bool _disposed;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="Database"/>.
        /// </summary>
        /// <param name="dbProvider">Data base provider instance.</param>
        /// <param name="databaseSettings">The configuration section.</param>
        public Database(IDbProvider dbProvider, DatabaseSettings databaseSettings) {
            Prevent.ParameterNull(dbProvider, nameof(dbProvider));
            Prevent.ParameterNull(databaseSettings, nameof(databaseSettings));

            _dbProvider = dbProvider;
            _databaseSettings = databaseSettings;
        }

        #endregion Public Constructors

        #region Destructor

        /// <summary>
        /// Destructor
        /// </summary>
        ~Database() {
            Dispose(false);
        }

        #endregion Destructor

        #region Private Methods

        private DbProviderFactory GetFactory() {
            if (_factory == null) {
                _factory = _dbProvider.GetFactory(_databaseSettings.ProviderName);
            }

            return _factory;
        }

        private DbConnection GetConnection() {
            if (_connection == null) {
                _connection = GetFactory().CreateConnection();

                _connection.ConnectionString = _databaseSettings.ConnectionString;
                _connection.Open();
            }

            return _connection;
        }

        private DbParameter ConvertParameter(Parameter parameter) {
            var result = GetFactory().CreateParameter();

            result.ParameterName = (!parameter.Name.StartsWith("@") ? string.Concat("@", parameter.Name) : parameter.Name);
            result.DbType = parameter.Type;
            result.Direction = parameter.Direction;
            result.Value = (parameter.Value != null ? parameter.Value : DBNull.Value);

            return result;
        }

        private void EnsureAccessBlockedAfterDispose() {
            if (_disposed) {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        private void Dispose(bool disposing) {
            if (_disposed) { return; }
            if (disposing) {
                if (_connection != null) {
                    if (_connection.State == ConnectionState.Open) {
                        _connection.Close();
                    }
                    _connection.Dispose();
                }
            }

            _factory = null;
            _connection = null;
            _disposed = true;
        }

        private void PrepareCommand(DbCommand command, string commandText, CommandType commandType, Parameter[] parameters) {
            command.CommandText = commandText;
            command.CommandType = commandType;

            parameters.Each(parameter => command.Parameters.Add(ConvertParameter(parameter)));
        }

        private object Execute(string commandText, CommandType commandType, Parameter[] parameters, bool scalar) {
            using (var command = GetConnection().CreateCommand()) {
                PrepareCommand(command, commandText, commandType, parameters);

                var result = scalar
                    ? command.ExecuteScalar()
                    : command.ExecuteNonQuery();

                command.Parameters.OfType<DbParameter>()
                    .Where(dbParameter => dbParameter.Direction != ParameterDirection.Input)
                    .Each(dbParameter => {
                        parameters
                            .Single(parameter =>
                                parameter.Name == dbParameter.ParameterName &&
                                parameter.Direction == dbParameter.Direction)
                            .Value = dbParameter.Value;
                    });

                return result;
            }
        }

        #endregion Private Methods

        #region IDatabase Members

        /// <inheritdoc />
        public IDbConnection Connection {
            get { return _connection; }
        }

        /// <inheritdoc />
        public int ExecuteNonQuery(string commandText, CommandType commandType = CommandType.Text, params Parameter[] parameters) {
            EnsureAccessBlockedAfterDispose();

            return (int)Execute(commandText, commandType, parameters, false);
        }

        /// <inheritdoc />
        public IEnumerable<TResult> ExecuteReader<TResult>(string commandText, Func<IDataReader, TResult> mapper, CommandType commandType = CommandType.Text, params Parameter[] parameters) {
            EnsureAccessBlockedAfterDispose();

            using (var command = GetConnection().CreateCommand()) {
                PrepareCommand(command, commandText, commandType, parameters);

                using (var reader = command.ExecuteReader()) {
                    while (reader.Read()) {
                        yield return mapper(reader);
                    }
                }
            }
        }

        /// <inheritdoc />
        public object ExecuteScalar(string commandText, CommandType commandType = CommandType.Text, params Parameter[] parameters) {
            EnsureAccessBlockedAfterDispose();

            return Execute(commandText, commandType, parameters, true);
        }

        #endregion IDatabase Members

        #region IDisposable Members

        /// <inheritdoc />
        public void Dispose() {
            GC.SuppressFinalize(this);
            Dispose(true);
        }

        #endregion IDisposable Members
    }
}