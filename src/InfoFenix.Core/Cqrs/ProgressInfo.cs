namespace InfoFenix.Core.Cqrs {

    public sealed class ProgressArguments {

        #region Public Properties

        public string Message { get; set; }
        public int ActualStep { get; set; }
        public int TotalSteps { get; set; }

        #endregion Public Properties

        #region Private Constructors

        private ProgressArguments() {
        }

        #endregion Private Constructors

        #region Public Static Methods

        public static ProgressArguments Create(string message, int actualStep, int totalSteps) {
            return new ProgressArguments {
                Message = message,
                ActualStep = actualStep,
                TotalSteps = totalSteps
            };
        }

        #endregion Public Static Methods
    }
}