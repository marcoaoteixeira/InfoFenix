﻿using Autofac;
using InfoFenix.Core.Services;

namespace InfoFenix.Core.IoC {

    public sealed class ServicesServiceRegistration : ServiceRegistrationBase {

        #region Public Override Methods

        public override void Register() {
            Builder
                .RegisterType<ManagementService>()
                .As<IManagementService>()
                .SingleInstance();
        }

        #endregion Public Override Methods
    }
}