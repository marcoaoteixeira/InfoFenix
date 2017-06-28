namespace InfoFenix.Core.Cqrs {

    public enum ProgressState {
        None,
        Start,
        PerformStep,
        Complete,
        Cancel,
        Error
    }

    public sealed class ProgressInfo {

        #region Public Properties

        public string Title { get; set; }
        public string Message { get; set; }
        public int ActualStep { get; set; }
        public int TotalSteps { get; set; }
        public ProgressState State { get; set; }

        #endregion Public Properties
    }
}