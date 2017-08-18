using System;

namespace InfoFenix.Logging {
    /// <summary>
    /// A null object pattern implementation of <see cref="ILoggerFactory"/>.
    /// </summary>
    public sealed class NullLoggerFactory : ILoggerFactory {

        #region ILoggerFactory Members

        /// <inheritdoc />
        public ILogger CreateLogger(Type type) {
            return NullLogger.Instance;
        }

        #endregion ILoggerFactory Members
    }
}