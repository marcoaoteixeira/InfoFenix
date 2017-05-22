using InfoFenix.Core.IoC;
using InfoFenix.Core.Office;

namespace InfoFenix.Core.Bootstrap.Actions {

    [Order(4)]
    public class InitializeComponentsAction : ActionBase {

        #region Private Read-Only Fields

        private readonly IResolver _resolver;

        #endregion Private Read-Only Fields

        #region Public Constructors

        /// <summary>
        /// Just initializes some components.
        /// </summary>
        /// <param name="documentDirectoryProcessor"></param>
        /// <param name="wordApplication"></param>
        public InitializeComponentsAction(IResolver resolver) {
            Prevent.ParameterNull(resolver, nameof(resolver));

            _resolver = resolver;
        }

        #endregion Public Constructors

        #region Public Override Methods

        public override void Execute() {
            _resolver.Resolve<IMicrosoftWordApplication>();
        }

        #endregion Public Override Methods
    }
}