using System.Reflection;
using Autofac;
using InfoFenix.Core.Cqrs;
using InfoFenix.Core.IoC;

namespace InfoFenix.Impl.IoC {

    public sealed class CqrsServiceRegistration : ServiceRegistrationBase {

        #region Public Constructors

        public CqrsServiceRegistration(params Assembly[] supportAssemblies)
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
               .RegisterType<CqrsDispatcher>()
               .As<ICqrsDispatcher>()
               .SingleInstance();
        }

        #endregion Public Override Methods
    }
}