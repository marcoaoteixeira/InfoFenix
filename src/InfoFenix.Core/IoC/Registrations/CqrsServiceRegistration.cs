using System.Reflection;
using Autofac;
using InfoFenix.Core.CQRS;

namespace InfoFenix.Core.IoC {

    public sealed class CQRSServiceRegistration : ServiceRegistrationBase {

        #region Public Constructors

        public CQRSServiceRegistration(params Assembly[] supportAssemblies)
            : base(supportAssemblies) { }

        #endregion Public Constructors

        #region Public Override Methods

        public override void Register() {
            Builder
                .RegisterAssemblyTypes(SupportAssemblies)
                .AsClosedTypesOf(typeof(ICommandHandler<>))
                .InstancePerDependency();

            Builder
                .RegisterAssemblyTypes(SupportAssemblies)
                .AsClosedTypesOf(typeof(IQueryHandler<,>))
                .InstancePerDependency();

            Builder
               .RegisterType<Mediator>()
               .As<IMediator>()
               .SingleInstance();
        }

        #endregion Public Override Methods
    }
}