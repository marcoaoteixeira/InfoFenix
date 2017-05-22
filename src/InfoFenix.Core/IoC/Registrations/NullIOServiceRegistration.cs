using Autofac;
using InfoFenix.Core.IO;
using InfoFenix.Core.IoC;

namespace InfoFenix.Core.IoC {
    public sealed class NullIOServiceRegistration : ServiceRegistrationBase {

        #region Public Override Methods

        public override void Register() {
            Builder
                .RegisterInstance(NullDirectoryWatcherManager.Instance)
                .As<IDirectoryWatcherManager>()
                .SingleInstance();
        }

        #endregion Public Override Methods
    }
}