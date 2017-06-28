using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Autofac;
using InfoFenix.Core;
using InfoFenix.Core.IoC;

namespace InfoFenix.Client.Code {

    public sealed class ClientServiceRegistration : ServiceRegistrationBase {

        #region Public Constructors

        public ClientServiceRegistration(params Assembly[] supportAssemblies)
            : base(supportAssemblies) { }

        #endregion Public Constructors

        #region Public Override Methods

        public override void Register() {
            var forms = SupportAssemblies.SelectMany(_ => _.ExportedTypes)
                .Where(_ => _.IsAssignableTo<Form>() && !_.IsAbstract)
                .ToArray();
            Builder
                .RegisterTypes(forms)
                .AsSelf()
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