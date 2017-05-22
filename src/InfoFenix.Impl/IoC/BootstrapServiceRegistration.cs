﻿using System.Linq;
using System.Reflection;
using Autofac;
using InfoFenix.Core.Bootstrap;
using InfoFenix.Core.IoC;

namespace InfoFenix.Core.IoC {
    public sealed class BootstrapServiceRegistration : ServiceRegistrationBase {

        #region Public Constructors

        public BootstrapServiceRegistration(params Assembly[] supportAssemblies)
            : base(supportAssemblies) { }

        #endregion Public Constructors

        #region Public Override Methods

        public override void Register() {
            Builder
                .RegisterType<Bootstrapper>()
                .As<IBootstrapper>()
                .SingleInstance();

            Builder
                .RegisterAssemblyTypes(SupportAssemblies)
                .Where(_ => _.IsAssignableTo<IAction>())
                .As<IAction>()
                .InstancePerDependency();
        }

        #endregion Public Override Methods
    }
}