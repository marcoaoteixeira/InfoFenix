using Autofac;
using InfoFenix.Core.IoC;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using InfoFenix.Core;

namespace InfoFenix.Client.Code {

    public sealed class ClientServiceRegistration : ServiceRegistrationBase {

        #region Public Constructors

        public ClientServiceRegistration(params Assembly[] supportAssemblies)
            : base(supportAssemblies) { }

        #endregion Public Constructors

        #region Public Override Methods

        public override void Register() {
            Builder
                .RegisterAssemblyTypes(SupportAssemblies)
                .Where(_ => _.IsAssignableTo<Form>())
                .AsSelf()
                .PropertiesAutowired()
                .InstancePerDependency();

            Builder
                .RegisterType<FormManager>()
                .As<IFormManager>()
                .SingleInstance();

            Builder
                .RegisterInstance(new CancellationTokenIssuer())
                .As<CancellationTokenIssuer>()
                .SingleInstance();
        }

        #endregion Public Override Methods
    }
}