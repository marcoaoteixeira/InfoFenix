using System;
using System.IO;
using log4net;
using log4net.Config;

namespace InfoFenix.Core.Logging {
    /// <summary>
    /// log4net implementation of <see cref="ILoggerFactory"/>
    /// </summary>
    public sealed class LoggerFactory : ILoggerFactory {

        #region Public Constants Fields

        /// <summary>
        /// Gets the default log4net configuration file name.
        /// </summary>
        public const string DefaultConfigFileName = "log4net.config";

        #endregion Public Constants Fields

        #region Private Static Fields

        // Logger factory should be watch
        // only one file and be attached to only
        // one configuration file.
        private static bool _configureAndWatchReady;

        #endregion Private Static Fields

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="LoggerFactory"/>
        /// </summary>
        /// <param name="configFileName">The configuration file name.</param>
        public LoggerFactory(string configFileName = DefaultConfigFileName) {
            if (_configureAndWatchReady) { return; }

            var file = string.IsNullOrWhiteSpace(configFileName)
                ? DefaultConfigFileName
                : configFileName;

            XmlConfigurator.ConfigureAndWatch(GetConfigFile(file));

            _configureAndWatchReady = true;
        }

        #endregion Public Constructors

        #region Private Static Methods

        private static FileInfo GetConfigFile(string configFilePath) {
            return (!Path.IsPathRooted(configFilePath)
                ? new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configFilePath))
                : new FileInfo(configFilePath));
        }

        #endregion Private Static Methods

        #region ILoggerFactory Members

        /// <inheritdoc />
        public ILogger CreateLogger(Type type) {
            return new Logger(LogManager.GetLogger(type));
        }

        #endregion ILoggerFactory Members
    }
}