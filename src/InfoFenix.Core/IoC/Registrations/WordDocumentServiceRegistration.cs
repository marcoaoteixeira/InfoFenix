﻿using Autofac;
using InfoFenix.Core.Office;

namespace InfoFenix.Core.IoC {

    public sealed class WordDocumentServiceRegistration : ServiceRegistrationBase {

        #region Public Override Methods

        public override void Register() {
            Builder
                .RegisterType<SpireDocWordApplication>()
                .As<IWordApplication>()
                .SingleInstance();
        }

        #endregion Public Override Methods
    }
}