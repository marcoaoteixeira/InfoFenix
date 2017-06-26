using System;
using System.Text;
using System.Threading.Tasks;
using InfoFenix.Core.Logging;
using Resource = InfoFenix.Core.Resources.Resources;

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

    public sealed class ProgressiveTaskArguments {

        #region Public Properties

        public string Title { get; set; }
        public string Message { get; set; }
        public string Error { get; set; }
        public int ActualStep { get; set; }
        public int TotalSteps { get; set; }

        public bool HasError {
            get { return !string.IsNullOrWhiteSpace(Error); }
        }

        #endregion Public Properties
    }

    public abstract class ProgressiveTaskNotification {

        #region Public Properties

        public ProgressiveTaskArguments Arguments { get; set; }

        #endregion Public Properties

        #region Protected Constructors

        protected ProgressiveTaskNotification() {
            Arguments = new ProgressiveTaskArguments();
        }

        #endregion Protected Constructors
    }

    public sealed class ProgressiveTaskStartNotification : ProgressiveTaskNotification {

        #region Public Constructors

        public ProgressiveTaskStartNotification() {
            Arguments.Message = Resource.ProgressiveTaskStartNotification_Message;
        }

        #endregion Public Constructors
    }

    public sealed class ProgressiveTaskPerformStepNotification : ProgressiveTaskNotification {
    }

    public sealed class ProgressiveTaskCompleteNotification : ProgressiveTaskNotification {

        #region Public Constructors

        public ProgressiveTaskCompleteNotification() {
            Arguments.Message = Resource.ProgressiveTaskCompleteNotification_Message;
        }

        #endregion Public Constructors
    }

    public sealed class ProgressiveTaskCancelNotification : ProgressiveTaskNotification {

        #region Public Constructors

        public ProgressiveTaskCancelNotification() {
            Arguments.Message = Resource.ProgressiveTaskCancelNotification_Message;
        }

        #endregion Public Constructors
    }

    public sealed class ProgressiveTaskErrorNotification : ProgressiveTaskNotification {

        #region Public Constructors

        public ProgressiveTaskErrorNotification() {
            Arguments.Message = Resources.Resources.ProgressiveTaskErrorNotification_Message;
        }

        #endregion Public Constructors
    }

    public sealed class ProgressiveTaskContinuationInfo {

        #region Public Properties

        public int ActualStep { get; set; }
        public int TotalSteps { get; set; }

        private ILogger _log;

        public ILogger Log {
            get { return _log ?? NullLogger.Instance; }
            set { _log = value ?? NullLogger.Instance; }
        }

        #endregion Public Properties
    }

    public static class ProgressiveTaskExtension {

        #region Public Static Methos

        public static void ProgressiveTaskStart(this IPublisherSubscriber source, string title = null, string message = null, int actualStep = 0, int totalSteps = 0, ILogger log = null) {
            InnerPublish<ProgressiveTaskStartNotification>(source, title, message, null, actualStep, totalSteps, log);
        }

        public static void ProgressiveTaskPerformStep(this IPublisherSubscriber source, string title = null, string message = null, int actualStep = 0, int totalSteps = 0, ILogger log = null) {
            InnerPublish<ProgressiveTaskPerformStepNotification>(source, title, message, null, actualStep, totalSteps, log);
        }

        public static void ProgressiveTaskComplete(this IPublisherSubscriber source, string title = null, string message = null, string error = null, int actualStep = 0, int totalSteps = 0, ILogger log = null) {
            InnerPublish<ProgressiveTaskCompleteNotification>(source, title, message, null, actualStep, totalSteps, log);
        }

        public static void ProgressiveTaskCancel(this IPublisherSubscriber source, string title = null, string message = null, int actualStep = 0, int totalSteps = 0, ILogger log = null) {
            InnerPublish<ProgressiveTaskCancelNotification>(source, title, message, null, actualStep, totalSteps, log);
        }

        public static void ProgressiveTaskError(this IPublisherSubscriber source, string error, string title = null, string message = null, int actualStep = 0, int totalSteps = 0, ILogger log = null) {
            InnerPublish<ProgressiveTaskErrorNotification>(source, title, message, null, actualStep, totalSteps, log);
        }
        
        public static void TaskContinuation(this IPublisherSubscriber source, Task continuation, object state) {
            if (source == null) { return; }
            if (continuation == null) { return; }
            if (state == null) { return; }

            var error = string.Empty;
            var info = (state as ProgressiveTaskContinuationInfo) ?? new ProgressiveTaskContinuationInfo();

            if (continuation.Exception is AggregateException ex) {
                var stringBuilder = new StringBuilder();
                foreach (var exception in ex.InnerExceptions) {
                    stringBuilder.AppendLine(exception.Message);
                }
                error = stringBuilder.ToString();
                source.ProgressiveTaskError(error: error, actualStep: info.ActualStep, totalSteps: info.TotalSteps, log: info.Log);
                return;
            }

            if (continuation.IsCanceled) {
                source.ProgressiveTaskCancel(actualStep: info.ActualStep, totalSteps: info.TotalSteps, log: info.Log);
                return;
            }

            source.ProgressiveTaskComplete(error: error, actualStep: info.ActualStep, totalSteps: info.TotalSteps, log: info.Log);
        }

        #endregion Public Static Methos

        #region Private Static Methods

        private static void InnerPublish<TNotification>(IPublisherSubscriber publisherSubscriber, string title, string message, string error, int actualStep, int totalSteps, ILogger log)
            where TNotification : ProgressiveTaskNotification, new() {
            var notification = new TNotification();

            log = log ?? NullLogger.Instance;

            if (title != null) { notification.Arguments.Title = title; }
            if (message != null) { notification.Arguments.Message = message; }
            if (error != null) { log.Error(error); }
            if (actualStep > -1) { notification.Arguments.ActualStep = actualStep; }
            if (totalSteps > -1) { notification.Arguments.TotalSteps = totalSteps; }

            notification.Arguments.Error = error;

            publisherSubscriber.Publish(notification);
        }

        #endregion Private Static Methods
    }

    #endregion Task Execution Notifications
}