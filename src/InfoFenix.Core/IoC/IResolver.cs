using System;

namespace InfoFenix.Core.IoC {
    public interface IResolver {

        #region Methods

        object Resolve(Type serviceType, string name = null);

        #endregion Methods
    }
}