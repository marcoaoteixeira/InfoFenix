﻿namespace InfoFenix.Core.Bootstrap.Actions {

    [Order(3)]
    public class InitializeServicesAction : ActionBase {

        #region IAction Members

        public override string Name => "Inicializar Serviços";

        public override void Execute() {
        }

        #endregion IAction Members
    }
}