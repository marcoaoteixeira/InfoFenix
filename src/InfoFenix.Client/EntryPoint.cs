using System;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using InfoFenix.Client.Code;
using InfoFenix.Core;
using InfoFenix.Core.Bootstrap;
using InfoFenix.Core.IoC;
using InfoFenix.Core.Logging;

namespace InfoFenix.Client {

    internal static class EntryPoint {

        #region Private Static Fields

        private static ICompositionRoot _compositionRoot;

        #endregion Private Static Fields

        #region Internal Static Properties

        internal static CancellationTokenIssuer CancellationTokenIssuer {
            get { return _compositionRoot.Resolver.Resolve<CancellationTokenIssuer>(); }
        }

        internal static bool IgnoreExitRoutine { get; set; }

        #endregion Internal Static Properties

        #region Internal Static Methods

        internal static void RestartApplication() {
            IgnoreExitRoutine = true;

            Application.Restart();
        }

        #endregion Internal Static Methods

        #region Private Static Methods

        /// <summary>
        /// Ponto de entrada principal para o aplicativo.
        /// </summary>
        [STAThread]
        private static void Main() {
            // Try solve problems with spire.doc
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ApplicationExit += (sender, e) => TearDown();

            ConfigureCompositionRoot();

            SplashScreenForm.Splash(() => _compositionRoot.Resolver.Resolve<SplashScreenForm>(), ExecuteBootstrapActions);
            Application.Run(_compositionRoot.Resolver.Resolve<MainForm>());
        }

        private static void ConfigureCompositionRoot() {
            var appSettings = AppSettingsManager.Get();
            var supportAssemblies = new[] {
                typeof(EntryPoint).Assembly,
                typeof(Prevent).Assembly
            };
            var defaultServiceRegistrations = new IServiceRegistration[] {
                new AppSettingsServiceRegistration(),
                new ClientServiceRegistration(supportAssemblies),
                new BootstrapServiceRegistration(supportAssemblies),
                new DataServiceRegistration(),
                new CqrsServiceRegistration(supportAssemblies),
                new PubSubServiceRegistration(),
                new SearchServiceRegistration(),
                new LoggingServiceRegistration(),
                new ServicesServiceRegistration()
            };
            var useRemoteSearchDatabaseServiceRegistrations = new IServiceRegistration[] {
                new NullIOServiceRegistration(),
                new NullWordDocumentServiceRegistration()
            };
            var autonomousModeServiceRegistrations = new IServiceRegistration[] {
                new IOServiceRegistration(),
                new WordDocumentServiceRegistration()
            };
            _compositionRoot = new CompositionRoot();
            _compositionRoot.Compose(defaultServiceRegistrations);
            _compositionRoot.Compose(appSettings.UseRemoteSearchDatabase ? useRemoteSearchDatabaseServiceRegistrations : autonomousModeServiceRegistrations);
            _compositionRoot.StartUp();
        }

        private static void ExecuteBootstrapActions() {
            _compositionRoot.Resolver.Resolve<IBootstrapper>().Run();
        }

        private static void TearDown() {
            var logger = _compositionRoot.Resolver.Resolve<ILoggerFactory>().CreateLogger(typeof(EntryPoint));

            try { CancellationTokenIssuer.CancelAll(); } catch (Exception ex) { logger.Error(ex, ex.Message); }

            _compositionRoot.TearDown();
            _compositionRoot = null;
        }

        #endregion Private Static Methods
    }
}