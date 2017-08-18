using System;
using System.Globalization;
using System.Reflection;
using System.Threading;
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
            // Try solve problems with spire.doc
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

            WinFormsApplication.EnableVisualStyles();
            WinFormsApplication.SetCompatibleTextRenderingDefault(false);
            WinFormsApplication.ApplicationExit += TearDown;
            WinFormsApplication.ThreadException += Error;

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

        private static void Error(object sender, ThreadExceptionEventArgs e) {
            if (e.Exception != null) {
                var logger = _compositionRoot.Resolver.Resolve<ILoggerFactory>().CreateLogger(typeof(EntryPoint));
                logger.Error(e.Exception, e.Exception.Message);
                MessageBox.Show($"Ocorreu um erro inesperado: {e.Exception.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void TearDown(object sender, EventArgs e) {
            var logger = _compositionRoot.Resolver.Resolve<ILoggerFactory>().CreateLogger(typeof(EntryPoint));
            var cancellationTokenIssuer = _compositionRoot.Resolver.Resolve<CancellationTokenIssuer>();

            try { cancellationTokenIssuer.CancelAll(); } catch (Exception ex) { logger.Error(ex, ex.Message); }

            _compositionRoot.TearDown();
            _compositionRoot = null;
        }

        #endregion Private Static Methods
    }
}