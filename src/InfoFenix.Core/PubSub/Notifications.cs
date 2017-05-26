using System;

namespace InfoFenix.Core.PubSub {

    public abstract class NotificationBase {
        #region Public Properties

        public string Title { get; set; }
        public string Message { get; set; }
        public string Error { get; set; }
        public NotificationState State { get; set; }

        #endregion

        #region Public Enum

        public enum NotificationState {
            None,
            Begin,
            Error,
            Done
        }

        #endregion
    }

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

    public sealed class StartingDocumentDirectoryIndexingNotification : NotificationBase {

        #region Public Properties

        public string FullPath { get; set; }
        public int TotalDocuments { get; set; }

        #endregion Public Properties
    }

    public sealed class DocumentDirectoryIndexingCompleteNotification : NotificationBase {

        #region Public Properties

        public string FullPath { get; set; }
        public int TotalDocuments { get; set; }

        #endregion Public Properties
    }

    public sealed class StartingDocumentIndexingNotification : NotificationBase {

        #region Public Properties

        public string FullPath { get; set; }
        public int TotalDocuments { get; set; }

        #endregion Public Properties
    }

    public sealed class DocumentIndexingCompleteNotification : NotificationBase {

        #region Public Properties

        public string FullPath { get; set; }

        #endregion Public Properties
    }

    public sealed class DocumentsInDirectoryToSaveNotification : NotificationBase {

        #region Public Properties

        public string DocumentDirectoryLabel { get; set; }
        public int TotalDocuments { get; set; }

        #endregion Public Properties
    }

    public sealed class DocumentSavedNotification : NotificationBase {

        #region Public Properties

        public string FullPath { get; set; }

        #endregion Public Properties
    }

    public sealed class DocumentsInDirectoryToRemoveNotification : NotificationBase {

        #region Public Properties

        public string DocumentDirectoryLabel { get; set; }
        public int TotalDocuments { get; set; }

        #endregion Public Properties
    }

    public sealed class DocumentRemovedNotification : NotificationBase {

        #region Public Properties

        public string FullPath { get; set; }

        #endregion Public Properties
    }

    public sealed class StartingProgressiveTaskNotification : NotificationBase {
        #region Public Properties

        public int MinimunValue { get; set; }
        public int MaximunValue { get; set; }

        #endregion
    }

    public sealed class ProgressiveTaskCompletedNotification : NotificationBase {
        #region Public Properties

        public int TotalSteps { get; set; }

        #endregion
    }

    public sealed class ProgressiveTaskPerformStepNotification : NotificationBase {
        #region Public Properties

        public int ActualStep { get; set; }
        public int StepsToCompletation { get; set; }

        #endregion
    }

    public sealed class SaveDocumentNotification : NotificationBase {
    }
}