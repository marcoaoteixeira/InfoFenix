using System;

namespace InfoFenix.Core.PubSub {

    public abstract class NotificationBase {

        #region Public Properties

        public string Title { get; set; }
        public string Message { get; set; }
        public string Error { get; set; }

        public bool HasError {
            get { return !string.IsNullOrWhiteSpace(Error); }
        }

        #endregion Public Properties
    }

    #region I/O Notifications

    public sealed class DirectoryContentChangeNotification : NotificationBase {

        #region Public Properties

        public string WatchingPath { get; set; }
        public string FullPath { get; set; }
        public ChangeTypes Changes { get; set; }

        #endregion Public Properties

        #region Public Enums

        [Flags]
        public enum ChangeTypes {
            Created = 1,
            Deleted = 2,
            Changed = 4,
            Renamed = 8,
            All = 15
        }

        #endregion Public Enums
    }

    #endregion I/O Notifications

    #region Task Execution Notifications

    public abstract class ProgressiveTaskNotification : NotificationBase {

        #region Public Properties

        public int ActualStep { get; set; }
        public int TotalSteps { get; set; }

        #endregion Public Properties
    }

    public sealed class ProgressiveTaskStartNotification : ProgressiveTaskNotification {

        #region Public Constructors

        public ProgressiveTaskStartNotification() {
            Message = Resources.Resources.ProgressiveTaskStartNotification_Message;
        }

        #endregion Public Constructors
    }

    public sealed class ProgressiveTaskPerformStepNotification : ProgressiveTaskNotification {
    }

    public sealed class ProgressiveTaskCompleteNotification : ProgressiveTaskNotification {

        #region Public Constructors

        public ProgressiveTaskCompleteNotification() {
            Message = Resources.Resources.ProgressiveTaskCompleteNotification_Message;
        }

        #endregion Public Constructors
    }

    public sealed class ProgressiveTaskCancelNotification : ProgressiveTaskNotification {

        #region Public Constructors

        public ProgressiveTaskCancelNotification() {
            Message = Resources.Resources.ProgressiveTaskCancelNotification_Message;
        }

        #endregion Public Constructors
    }

    public sealed class ProgressiveTaskErrorNotification : ProgressiveTaskNotification {

        #region Public Constructors

        public ProgressiveTaskErrorNotification() {
            Message = Resources.Resources.ProgressiveTaskErrorNotification_Message;
        }

        #endregion Public Constructors
    }

    #endregion Task Execution Notifications
}