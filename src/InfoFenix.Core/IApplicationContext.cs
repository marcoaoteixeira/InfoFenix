namespace InfoFenix.Core {

    public interface IApplicationContext {

        #region Properties

        IAppSettings Settings { get; }
        CancellationTokenIssuer Issuer { get; }

        #endregion Properties
    }

    public class ApplicationContext : IApplicationContext {

        #region Public Constructors

        public ApplicationContext(IAppSettings settings, CancellationTokenIssuer issuer) {
            Settings = settings;
            Issuer = issuer;
        }

        #endregion Public Constructors

        #region IApplicationContext Members

        public IAppSettings Settings { get; }

        public CancellationTokenIssuer Issuer { get; }

        #endregion IApplicationContext Members
    }
}