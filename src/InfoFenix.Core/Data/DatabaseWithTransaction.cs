using System;
using System.Collections.Generic;
using System.Data;

namespace InfoFenix.Core.Data {

    internal class DatabaseWithTransaction : IDatabase, IDisposable {

        #region Private Fields

        private IDatabase _decorator;
        private IDbTransaction _transaction;
        private bool _disposed;

        #endregion Private Fields

        #region Internal Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="DatabaseWithTransaction"/>
        /// </summary>
        /// <param name="decorator">The instance of <see cref="IDatabase"/> to decorate.</param>
        internal DatabaseWithTransaction(IDatabase decorator) {
            Prevent.ParameterNull(decorator, nameof(decorator));

            _decorator = decorator;
            _transaction = _decorator.Connection.BeginTransaction(IsolationLevel.ReadUncommitted);
        }

        #endregion Public Constructors

        #region Destructor

        /// <summary>
        /// Destructor.
        /// </summary>
        ~DatabaseWithTransaction() {
            Dispose(false);
        }

        #endregion Destructor

        #region Private Methods

        private void Dispose(bool disposing) {
            if (_disposed) { return; }
            if (disposing) {
                if (_transaction != null) {
                    try { _transaction.Commit(); }
                    catch { _transaction.Rollback(); }
                    finally { _transaction.Dispose(); }
                }
            }

            _transaction = null;
            _decorator = null;
            _disposed = true;
        }

        #endregion Private Methods

        #region IDatabase Members

        /// <inheritdoc />
        IDbConnection IDatabase.Connection {
            get { return _decorator.Connection; }
        }

        /// <inheritdoc />
        int IDatabase.ExecuteNonQuery(string commandText, CommandType commandType, params Parameter[] parameters) {
            return _decorator.ExecuteNonQuery(commandText, commandType, parameters);
        }

        /// <inheritdoc />
        IEnumerable<TResult> IDatabase.ExecuteReader<TResult>(string commandText, Func<IDataReader, TResult> mapper, CommandType commandType, params Parameter[] parameters) {
            return _decorator.ExecuteReader(commandText, mapper, commandType, parameters);
        }

        /// <inheritdoc />
        object IDatabase.ExecuteScalar(string commandText, CommandType commandType, params Parameter[] parameters) {
            return _decorator.ExecuteScalar(commandText, commandType, parameters);
        }

        #endregion IDatabase Members

        #region IDisposable Members

        /// <inheritdoc />
        void IDisposable.Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable Members
    }
}