namespace InfoFenix.Core.IoC {
    public interface ICompositionRoot {

        #region Properties

        IResolver Resolver { get; }

        #endregion Properties

        #region Methods

        void Compose(params IServiceRegistration[] registrations);
        void StartUp();
        void TearDown();

        #endregion Methods
    }
}