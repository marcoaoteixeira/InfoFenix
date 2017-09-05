using System;
using System.Reflection;
using System.Windows.Forms;
using InfoFenix.Application.Code;
using InfoFenix.Application.Views;
using InfoFenix.Application.Views.Home;
using InfoFenix.Bootstrap;
using InfoFenix.Infrastructure;
using InfoFenix.IoC;
using InfoFenix.Logging;
using WinFormsApplication = System.Windows.Forms.Application;

namespace InfoFenix.Application {

    internal static class EntryPoint {

        #region Private Static Fields

        private static ICompositionRoot _compositionRoot;

        #endregion Private Static Fields

        #region Internal Static Properties

        internal static bool IgnoreExitRoutine { get; set; }

        internal static Version ApplicationVersion => typeof(EntryPoint).Assembly.GetName().Version;

        #endregion Internal Static Properties

        #region Internal Static Methods

        internal static void RestartApplication() {
            IgnoreExitRoutine = true;

            WinFormsApplication.Restart();
        }

        #endregion Internal Static Methods

        #region Private Static Methods

        /// <summary>
        /// Ponto de entrada principal para o aplicativo.
        /// </summary>
        [STAThread]
        private static void Main() {
            WinFormsApplication.EnableVisualStyles();
            WinFormsApplication.SetCompatibleTextRenderingDefault(false);
            AppDomain.CurrentDomain.UnhandledException += (sender, e) => TearDown(e.IsTerminating, e.ExceptionObject as Exception);
            WinFormsApplication.ApplicationExit += (sender, e) => TearDown(isTerminating: false, ex: null);

            // Initialize AppSettings file
            AppSettings.Instance.Save();

            ConfigureCompositionRoot();

            SplashScreenForm.Splash(() => _compositionRoot.Resolver.Resolve<SplashScreenForm>(), ExecuteBootstrapActions);
            WinFormsApplication.Run(_compositionRoot.Resolver.Resolve<MainForm>());
        }

        private static void ConfigureCompositionRoot() {
            var supportAssemblies = new[] {
                Assembly.Load("InfoFenix.Bootstrap"),
                Assembly.Load("InfoFenix.Configuration"),
                Assembly.Load("InfoFenix.CQRS"),
                Assembly.Load("InfoFenix.Data"),
                Assembly.Load("InfoFenix.Domains"),
                Assembly.Load("InfoFenix.IoC"),
                Assembly.Load("InfoFenix.Logging"),
                Assembly.Load("InfoFenix.Migrations"),
                Assembly.Load("InfoFenix.Office"),
                Assembly.Load("InfoFenix.PubSub"),
                Assembly.Load("InfoFenix.Search"),
                Assembly.Load("InfoFenix.Services"),
                typeof(EntryPoint).Assembly
            };
            var defaultServiceRegistrations = new IServiceRegistration[] {
                new ClientServiceRegistration(supportAssemblies),
                new BootstrapServiceRegistration(supportAssemblies),
                new DataServiceRegistration(),
                new CQRSServiceRegistration(supportAssemblies),
                new PubSubServiceRegistration(),
                new SearchServiceRegistration(),
                new LoggingServiceRegistration(),
                new ServicesServiceRegistration()
            };
            var useRemoteSearchDatabaseServiceRegistrations = new IServiceRegistration[] {
                new NullWordDocumentServiceRegistration()
            };
            var autonomousModeServiceRegistrations = new IServiceRegistration[] {
                new WordDocumentServiceRegistration()
            };
            _compositionRoot = new CompositionRoot();
            _compositionRoot.Compose(defaultServiceRegistrations);
            _compositionRoot.Compose(AppSettings.Instance.UseRemoteSearchDatabase ? useRemoteSearchDatabaseServiceRegistrations : autonomousModeServiceRegistrations);
            _compositionRoot.StartUp();
        }

        private static void ExecuteBootstrapActions() {
            _compositionRoot.Resolver.Resolve<IBootstrapper>().Run();
        }

        private static void TearDown(bool isTerminating, Exception ex) {
            var logger = _compositionRoot.Resolver.Resolve<ILoggerFactory>().CreateLogger(typeof(EntryPoint));

            if (isTerminating) {
                if (ex != null) { logger.Error(ex, "*** FATAL ERROR!!! TERMINATING APPLICATION!!! ***"); }
                else { logger.Error("*** FATAL ERROR!!! TERMINATING APPLICATION!!! ***"); }

                MessageBox.Show(
                    text: ex != null
                        ? $"Ocorreu um erro irrecuperável ({ex.Message}). O aplicativo será finalizado."
                        : $"Ocorreu um erro irrecuperável. O aplicativo será finalizado.",
                    caption: "Erro",
                    buttons: MessageBoxButtons.OK,
                    icon: MessageBoxIcon.Error);
            }

            try {
                _compositionRoot
                    .Resolver
                    .Resolve<CancellationTokenIssuer>()
                    .CancelAll();
            } catch (Exception e) { logger.Error(e, e.Message); }

            _compositionRoot.TearDown();
            _compositionRoot = null;
        }

        #endregion Private Static Methods
    }
}