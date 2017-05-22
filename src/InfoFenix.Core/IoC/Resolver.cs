using System;
using Autofac;

namespace InfoFenix.Core.IoC {

    public sealed class Resolver : IResolver {

        #region Private Read-Only Fields

        private readonly ILifetimeScope _scope;

        #endregion Private Read-Only Fields

        #region Public Constructors

        public Resolver(ILifetimeScope scope) {
            Prevent.ParameterNull(scope, nameof(scope));

            _scope = scope;
        }

        #endregion Public Constructors

        #region IResolver Members

        public object Resolve(Type serviceType, string name = null) {
            Prevent.ParameterNull(serviceType, nameof(serviceType));

            return !string.IsNullOrWhiteSpace(name)
                ? _scope.ResolveNamed(name, serviceType)
                : _scope.Resolve(serviceType);
        }

        #endregion IResolver Members
    }
}