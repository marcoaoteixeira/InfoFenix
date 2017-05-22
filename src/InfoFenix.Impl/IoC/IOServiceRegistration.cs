using Autofac;
using InfoFenix.Core.IO;
using InfoFenix.Core.IoC;

namespace InfoFenix.Impl.IoC {

    public sealed class IOServiceRegistration : ServiceRegistrationBase {

        #region Public Override Methods

        public override void Register() {
            Builder
                .RegisterType<DirectoryWatcherManager>()
                .As<IDirectoryWatcherManager>()
                .SingleInstance();

            Builder
                .RegisterType<DirectoryWatcherFactory>()
                .As<IDirectoryWatcherFactory>()
                .SingleInstance();

            Builder
                .RegisterType<DirectoryWatcher>()
                .As<IDirectoryWatcher>()
                .InstancePerDependency();
        }

        #endregion Public Override Methods
    }
}