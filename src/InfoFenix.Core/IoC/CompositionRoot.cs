using System;
using Autofac;

namespace InfoFenix.Core.IoC {

    public class CompositionRoot : ICompositionRoot, IDisposable {

        #region Private Fields

        private ContainerBuilder _builder;
        private IContainer _container;
        private bool _disposed;

        #endregion Private Fields

        #region Destructor

        ~CompositionRoot() {
            Dispose(disposing: false);
        }

        #endregion Destructor

        #region Private Methods

        private ContainerBuilder GetBuilder() {
            return _builder ?? (_builder = new ContainerBuilder());
        }

        private void Dispose(bool disposing) {
            if (_disposed) { return; }
            if (disposing) { TearDown(); }
            _disposed = true;
        }

        #endregion Private Methods

        #region ICompositionRoot Members

        public IResolver Resolver {
            get {
                if (_container == null) {
                    throw new InvalidOperationException("Container not builded.");
                }
                return _container.Resolve<IResolver>();
            }
        }

        public void Compose(params IServiceRegistration[] registrations) {
            if (_container != null) {
                throw new InvalidOperationException("Container already builded.");
            }

            foreach (var registration in registrations) {
                var innerRegistration = registration as ServiceRegistrationBase;
                if (innerRegistration != null) {
                    innerRegistration.SetBuilder(GetBuilder());
                }
                registration.Register();
            }
        }

        public void StartUp() {
            if (_container != null) {
                throw new InvalidOperationException("Container already builded.");
            }

            GetBuilder()
                .RegisterType<Resolver>()
                .As<IResolver>()
                .PreserveExistingDefaults();

            _container = GetBuilder().Build();
        }

        public void TearDown() {
            if (_container != null) {
                _container.Dispose();
            }

            _container = null;
            _builder = null;
        }

        #endregion ICompositionRoot Members

        #region IDisposable Members

        public void Dispose() {
            Dispose(disposing: true);
            GC.SuppressFinalize(obj: this);
        }

        #endregion IDisposable Members
    }
}