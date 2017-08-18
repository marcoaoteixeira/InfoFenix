using System;

namespace InfoFenix.Logging {
    /// <summary>
    /// <see cref="ILogger"/> extension methods.
    /// </summary>
    public static class LoggerExtension {

        #region Public Static Methods

        #region Debug Log Methods

        /// <summary>
        /// Writes a debug log line.
        /// </summary>
        /// <param name="source">The <see cref="ILogger"/> instance.</param>
        /// <param name="message">The message to log.</param>
        public static void Debug(this ILogger source, string message) {
            FilteredLog(source, LogLevel.Debug, null, message, null);
        }

        /// <summary>
        /// Writes a debug log line.
        /// </summary>
        /// <param name="source">The <see cref="ILogger"/> instance.</param>
        /// <param name="exception">The <see cref="Exception"/> to log.</param>
        /// <param name="message">The message to log.</param>
        public static void Debug(this ILogger source, Exception exception, string message) {
            FilteredLog(source, LogLevel.Debug, exception, message, null);
        }

        /// <summary>
        /// Writes a debug log line.
        /// </summary>
        /// <param name="source">The <see cref="ILogger"/> instance.</param>
        /// <param name="format">The message format to log.</param>
        /// <param name="args">The message format arguments, if any.</param>
        public static void Debug(this ILogger source, string format, params object[] args) {
            FilteredLog(source, LogLevel.Debug, null, format, args);
        }

        /// <summary>
        /// Writes a debug log line.
        /// </summary>
        /// <param name="source">The <see cref="ILogger"/> instance.</param>
        /// <param name="exception">The <see cref="Exception"/> to log.</param>
        /// <param name="format">The message format to log.</param>
        /// <param name="args">The message format arguments, if any.</param>
        public static void Debug(this ILogger source, Exception exception, string format, params object[] args) {
            FilteredLog(source, LogLevel.Debug, exception, format, args);
        }

        #endregion Debug Log Methods

        #region Information Log Methods

        /// <summary>
        /// Writes an information log line.
        /// </summary>
        /// <param name="source">The <see cref="ILogger"/> instance.</param>
        /// <param name="message">The message to log.</param>
        public static void Information(this ILogger source, string message) {
            FilteredLog(source, LogLevel.Information, null, message, null);
        }

        /// <summary>
        /// Writes an information log line.
        /// </summary>
        /// <param name="source">The <see cref="ILogger"/> instance.</param>
        /// <param name="exception">The <see cref="Exception"/> to log.</param>
        /// <param name="message">The message to log.</param>
        public static void Information(this ILogger source, Exception exception, string message) {
            FilteredLog(source, LogLevel.Information, exception, message, null);
        }

        /// <summary>
        /// Writes an information log line.
        /// </summary>
        /// <param name="source">The <see cref="ILogger"/> instance.</param>
        /// <param name="format">The message format to log.</param>
        /// <param name="args">The message format arguments, if any.</param>
        public static void Information(this ILogger source, string format, params object[] args) {
            FilteredLog(source, LogLevel.Information, null, format, args);
        }

        /// <summary>
        /// Writes an information log line.
        /// </summary>
        /// <param name="source">The <see cref="ILogger"/> instance.</param>
        /// <param name="exception">The <see cref="Exception"/> to log.</param>
        /// <param name="format">The message format to log.</param>
        /// <param name="args">The message format arguments, if any.</param>
        public static void Information(this ILogger source, Exception exception, string format, params object[] args) {
            FilteredLog(source, LogLevel.Information, exception, format, args);
        }

        #endregion Information Log Methods

        #region Warning Log Methods

        /// <summary>
        /// Writes a warning log line.
        /// </summary>
        /// <param name="source">The <see cref="ILogger"/> instance.</param>
        /// <param name="message">The message to log.</param>
        public static void Warning(this ILogger source, string message) {
            FilteredLog(source, LogLevel.Warning, null, message, null);
        }

        /// <summary>
        /// Writes a warning log line.
        /// </summary>
        /// <param name="source">The <see cref="ILogger"/> instance.</param>
        /// <param name="exception">The <see cref="Exception"/> to log.</param>
        /// <param name="message">The message to log.</param>
        public static void Warning(this ILogger source, Exception exception, string message) {
            FilteredLog(source, LogLevel.Warning, exception, message, null);
        }

        /// <summary>
        /// Writes a warning log line.
        /// </summary>
        /// <param name="source">The <see cref="ILogger"/> instance.</param>
        /// <param name="format">The message format to log.</param>
        /// <param name="args">The message format arguments, if any.</param>
        public static void Warning(this ILogger source, string format, params object[] args) {
            FilteredLog(source, LogLevel.Warning, null, format, args);
        }

        /// <summary>
        /// Writes a warning log line.
        /// </summary>
        /// <param name="source">The <see cref="ILogger"/> instance.</param>
        /// <param name="exception">The <see cref="Exception"/> to log.</param>
        /// <param name="format">The message format to log.</param>
        /// <param name="args">The message format arguments, if any.</param>
        public static void Warning(this ILogger source, Exception exception, string format, params object[] args) {
            FilteredLog(source, LogLevel.Warning, exception, format, args);
        }

        #endregion Warning Log Methods

        #region Error Log Methods

        /// <summary>
        /// Writes an error log line.
        /// </summary>
        /// <param name="source">The <see cref="ILogger"/> instance.</param>
        /// <param name="message">The message to log.</param>
        public static void Error(this ILogger source, string message) {
            FilteredLog(source, LogLevel.Error, null, message, null);
        }

        /// <summary>
        /// Writes an error log line.
        /// </summary>
        /// <param name="source">The <see cref="ILogger"/> instance.</param>
        /// <param name="exception">The <see cref="Exception"/> to log.</param>
        /// <param name="message">The message to log.</param>
        public static void Error(this ILogger source, Exception exception, string message) {
            FilteredLog(source, LogLevel.Error, exception, message, null);
        }

        /// <summary>
        /// Writes an error log line.
        /// </summary>
        /// <param name="source">The <see cref="ILogger"/> instance.</param>
        /// <param name="format">The message format to log.</param>
        /// <param name="args">The message format arguments, if any.</param>
        public static void Error(this ILogger source, string format, params object[] args) {
            FilteredLog(source, LogLevel.Error, null, format, args);
        }

        /// <summary>
        /// Writes an error log line.
        /// </summary>
        /// <param name="source">The <see cref="ILogger"/> instance.</param>
        /// <param name="exception">The <see cref="Exception"/> to log.</param>
        /// <param name="format">The message format to log.</param>
        /// <param name="args">The message format arguments, if any.</param>
        public static void Error(this ILogger source, Exception exception, string format, params object[] args) {
            FilteredLog(source, LogLevel.Error, exception, format, args);
        }

        #endregion Error Log Methods

        #region Fatal Log Methods

        /// <summary>
        /// Writes a fatal log line.
        /// </summary>
        /// <param name="source">The <see cref="ILogger"/> instance.</param>
        /// <param name="message">The message to log.</param>
        public static void Fatal(this ILogger source, string message) {
            FilteredLog(source, LogLevel.Fatal, null, message, null);
        }

        /// <summary>
        /// Writes a fatal log line.
        /// </summary>
        /// <param name="source">The <see cref="ILogger"/> instance.</param>
        /// <param name="exception">The <see cref="Exception"/> to log.</param>
        /// <param name="message">The message to log.</param>
        public static void Fatal(this ILogger source, Exception exception, string message) {
            FilteredLog(source, LogLevel.Fatal, exception, message, null);
        }

        /// <summary>
        /// Writes a fatal log line.
        /// </summary>
        /// <param name="source">The <see cref="ILogger"/> instance.</param>
        /// <param name="format">The message format to log.</param>
        /// <param name="args">The message format arguments, if any.</param>
        public static void Fatal(this ILogger source, string format, params object[] args) {
            FilteredLog(source, LogLevel.Fatal, null, format, args);
        }

        /// <summary>
        /// Writes a fatal log line.
        /// </summary>
        /// <param name="source">The <see cref="ILogger"/> instance.</param>
        /// <param name="exception">The <see cref="Exception"/> to log.</param>
        /// <param name="format">The message format to log.</param>
        /// <param name="args">The message format arguments, if any.</param>
        public static void Fatal(this ILogger source, Exception exception, string format, params object[] args) {
            FilteredLog(source, LogLevel.Fatal, exception, format, args);
        }

        #endregion Fatal Log Methods

        #endregion Public Static Methods

        #region Private Static Methods

        private static void FilteredLog(ILogger logger, LogLevel level, Exception exception, string format, object[] objects) {
            if (logger.IsEnabled(level)) {
                logger.Log(level, exception, format, objects);
            }
        }

        #endregion Private Static Methods
    }
}