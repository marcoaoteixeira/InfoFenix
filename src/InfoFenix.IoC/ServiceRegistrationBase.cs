using System.Linq;
using System.Reflection;
using Autofac;

namespace InfoFenix.IoC {

    public abstract class ServiceRegistrationBase : IServiceRegistration {

        #region Protected Properties

        protected ContainerBuilder Builder { get; private set; }
        protected Assembly[] SupportAssemblies { get; private set; }

        #endregion Protected Properties

        #region Protected Constructors

        protected ServiceRegistrationBase() {
            SupportAssemblies = Enumerable.Empty<Assembly>().ToArray();
        }

        protected ServiceRegistrationBase(params Assembly[] supportAssemblies) {
            SupportAssemblies = supportAssemblies;
        }

        #endregion Protected Constructors

        #region Public Methods

        public void SetBuilder(ContainerBuilder builder) {
            Builder = builder;
        }

        #endregion Public Methods

        #region IServiceRegistration Members

        public abstract void Register();

        #endregion IServiceRegistration Members
    }
}