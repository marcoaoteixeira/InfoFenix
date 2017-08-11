using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InfoFenix.Core.CQRS;
using InfoFenix.Core.Data;
using InfoFenix.Core.Logging;
using Resource = InfoFenix.Core.Resources.Resources;

namespace InfoFenix.Core.Migrations {

    public sealed class ApplyMigrationCommand : ICommand {

        #region Public Properties

        public string Version { get; set; }

        #endregion Public Properties
    }

    public sealed class ApplyMigrationCommandHandler : ICommandHandler<ApplyMigrationCommand> {

        #region Private Read-Only Fields

        private readonly IDatabase _database;

        #endregion Private Read-Only Fields

        #region Public Properties

        private ILogger _log;

        /// <summary>
        /// Gets or sets the Log value.
        /// </summary>
        public ILogger Log {
            get { return _log ?? (_log = NullLogger.Instance); }
            set { _log = value ?? NullLogger.Instance; }
        }

        #endregion Public Properties

        #region Public Constructors

        public ApplyMigrationCommandHandler(IDatabase database) {
            Prevent.ParameterNull(database, nameof(database));

            _database = database;
        }

        #endregion Public Constructors

        #region ICommandHandler<ApplyMigrationCommand> Members

        public Task HandleAsync(ApplyMigrationCommand command, CancellationToken cancellationToken = default(CancellationToken), IProgress<ProgressInfo> progress = null) {
            return Task.Run(() => {
                try {
                    var migration = _database.ExecuteReaderSingle(Resource.GetAppliedMigrationSQL, Migration.Map, parameters: new[] {
                        Parameter.CreateInputParameter(Common.DatabaseSchema.Migrations.Version, command.Version)
                    });

                    if (migration != null) { return; }
                } catch { /* IGNORE ERROR ON SEARCH */ }

                var migrationFiles = Common
                    .GetManifestResourceNames()
                    .Where(_ => _.StartsWith(Common.MIGRATION_FOLDER) && _.Contains(command.Version))
                    .OrderBy(_ => _);

                if (migrationFiles.IsNullOrEmpty()) { return; }

                using (var transaction = _database.Connection.BeginTransaction()) {
                    try {
                        var commandText = string.Empty;
                        foreach (var migrationFile in migrationFiles) {
                            using (var stream = Common.GetManifestResourceStream(migrationFile))
                            using (var streamReader = new StreamReader(stream)) {
                                commandText = streamReader.ReadToEnd();
                            }
                            cancellationToken.ThrowIfCancellationRequested();
                            _database.ExecuteNonQuery(commandText);
                            cancellationToken.ThrowIfCancellationRequested();
                        }

                        _database.ExecuteNonQuery(Resource.SaveMigrationSQL, parameters: new[] {
                            Parameter.CreateInputParameter(Common.DatabaseSchema.Migrations.Version, command.Version),
                            Parameter.CreateInputParameter(Common.DatabaseSchema.Migrations.Date, DateTime.Now, DbType.DateTime)
                        });

                        transaction.Commit();
                    } catch (Exception ex) { Log.Error(ex.Message); transaction.Rollback(); throw; }
                }
            }, cancellationToken);
        }

        #endregion ICommandHandler<ApplyMigrationCommand> Members
    }
}