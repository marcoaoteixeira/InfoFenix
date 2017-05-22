using System;

namespace InfoFenix.Core.Logging {
    /// <summary>
    /// A null object pattern implementation of <see cref="ILogger"/>.
    /// </summary>
    public sealed class NullLogger : ILogger {

        #region Private Static Fields

        private static readonly ILogger CurrentInstance = new NullLogger();

        #endregion Private Static Fields

        #region Public Static Properties

        /// <summary>
        /// Gets the current single instance of <see cref="NullLogger"/>.
        /// </summary>
        public static ILogger Instance {
            get { return CurrentInstance; }
        }

        #endregion Public Static Properties

        #region ILogger Members

        /// <inheritdoc />
        public bool IsEnabled(LogLevel level) {
            return false;
        }

        /// <inheritdoc />
        public void Log(LogLevel level, Exception exception, string format, params object[] args) { }

        #endregion ILogger Members
    }
}