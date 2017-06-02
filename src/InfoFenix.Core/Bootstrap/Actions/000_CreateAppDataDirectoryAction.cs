﻿using System.IO;
using InfoFenix.Core.PubSub;

namespace InfoFenix.Core.Bootstrap.Actions {

    [Order(0)]
    public class CreateAppDataDirectoryAction : ActionBase {

        #region Private Read-Only Fields

        private readonly IAppSettings _appSettings;

        #endregion Private Read-Only Fields

        #region Public Constructors

        public CreateAppDataDirectoryAction(IAppSettings appSettings, IPublisherSubscriber publisherSubscriber) {
            Prevent.ParameterNull(appSettings, nameof(appSettings));

            _appSettings = appSettings;
        }

        #endregion Public Constructors

        #region IAction Members

        public override string Description => "Inicializando diretório de dados para o aplicativo...";

        public override void Execute() {
            if (!Directory.Exists(_appSettings.ApplicationDataDirectoryPath)) {
                Directory.CreateDirectory(_appSettings.ApplicationDataDirectoryPath);
            }
        }

        #endregion IAction Members
    }
}