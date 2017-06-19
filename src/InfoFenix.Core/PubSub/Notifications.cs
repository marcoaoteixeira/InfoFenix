using System;
using System.Threading.Tasks;
using InfoFenix.Core.Logging;

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

        public ProgressiveTaskArguments Arguments { get; }

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
            Arguments.Message = Resources.Resources.ProgressiveTaskStartNotification_Message;
        }

        #endregion Public Constructors
    }

    public sealed class ProgressiveTaskPerformStepNotification : ProgressiveTaskNotification {
    }

    public sealed class ProgressiveTaskCompleteNotification : ProgressiveTaskNotification {

        #region Public Constructors

        public ProgressiveTaskCompleteNotification() {
            Arguments.Message = Resources.Resources.ProgressiveTaskCompleteNotification_Message;
        }

        #endregion Public Constructors
    }

    public sealed class ProgressiveTaskCancelNotification : ProgressiveTaskNotification {

        #region Public Constructors

        public ProgressiveTaskCancelNotification() {
            Arguments.Message = Resources.Resources.ProgressiveTaskCancelNotification_Message;
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

        public static Task ProgressiveTaskStartAsync(this IPublisherSubscriber source, string title = null, string message = null, int actualStep = 0, int totalSteps = 0, ILogger log = null) {
            return InnerPublishAsync<ProgressiveTaskStartNotification>(source, title, message, null, actualStep, totalSteps, log);
        }

        public static Task ProgressiveTaskPerformStepAsync(this IPublisherSubscriber source, string title = null, string message = null, int actualStep = 0, int totalSteps = 0, ILogger log = null) {
            return InnerPublishAsync<ProgressiveTaskPerformStepNotification>(source, title, message, null, actualStep, totalSteps, log);
        }

        public static Task ProgressiveTaskCompleteAsync(this IPublisherSubscriber source, string title = null, string message = null, string error = null, int actualStep = 0, int totalSteps = 0, ILogger log = null) {
            return InnerPublishAsync<ProgressiveTaskCompleteNotification>(source, title, message, null, actualStep, totalSteps, log);
        }

        public static Task ProgressiveTaskCancelAsync(this IPublisherSubscriber source, string title = null, string message = null, int actualStep = 0, int totalSteps = 0, ILogger log = null) {
            return InnerPublishAsync<ProgressiveTaskCancelNotification>(source, title, message, null, actualStep, totalSteps, log);
        }

        public static Task ProgressiveTaskErrorAsync(this IPublisherSubscriber source, string error, string title = null, string message = null, int actualStep = 0, int totalSteps = 0, ILogger log = null) {
            return InnerPublishAsync<ProgressiveTaskErrorNotification>(source, title, message, null, actualStep, totalSteps, log);
        }

        public static void TaskContinuation(this IPublisherSubscriber source, Task continuation, object state) {
            if (source == null) { return; }
            if (continuation == null) { return; }
            if (state == null) { return; }

            var info = (state as ProgressiveTaskContinuationInfo) ?? new ProgressiveTaskContinuationInfo();

            if (continuation.Exception != null) {
                source.ProgressiveTaskErrorAsync(error: continuation.Exception.Message, actualStep: info.ActualStep, totalSteps: info.TotalSteps, log: info.Log);
            }

            if (continuation.IsCanceled) {
                source.ProgressiveTaskCancelAsync(actualStep: info.ActualStep, totalSteps: info.TotalSteps, log: info.Log);
            }

            source.ProgressiveTaskCompleteAsync(error: continuation.Exception.Message, actualStep: info.ActualStep, totalSteps: info.TotalSteps, log: info.Log);
        }

        #endregion Public Static Methos

        #region Private Static Methods

        private static Task InnerPublishAsync<TNotification>(IPublisherSubscriber publisherSubscriber, string title, string message, string error, int actualStep, int totalSteps, ILogger log)
            where TNotification : ProgressiveTaskNotification, new() {
            var notification = new TNotification();

            log = log ?? NullLogger.Instance;

            if (title != null) { notification.Arguments.Title = title; }
            if (message != null) { notification.Arguments.Message = message; }
            if (error != null) { log.Error(error); }
            if (actualStep > -1) { notification.Arguments.ActualStep = actualStep; }
            if (totalSteps > -1) { notification.Arguments.TotalSteps = totalSteps; }

            notification.Arguments.Error = error;

            return publisherSubscriber.PublishAsync(notification);
        }

        #endregion Private Static Methods
    }

    #endregion Task Execution Notifications
}