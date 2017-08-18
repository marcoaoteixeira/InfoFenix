using System;
using InfoFenix.Resources;

namespace InfoFenix.CQRS {

    public static class ProgressExtension {

        #region Public Static Methods

        public static void Start(this IProgress<ProgressInfo> source, int totalSteps = 0, string title = null, params object[] parameters) {
            if (source == null) { return; }

            source.Report(new ProgressInfo {
                Title = !string.IsNullOrWhiteSpace(title)
                    ? !parameters.IsNullOrEmpty()
                        ? string.Format(title, parameters)
                        : title
                    : Strings.Progress_Start,
                TotalSteps = totalSteps,
                State = ProgressState.Start
            });
        }

        public static void PerformStep(this IProgress<ProgressInfo> source, int actualStep = 0, int totalSteps = 0, string message = null, params object[] parameters) {
            if (source == null) { return; }

            source.Report(new ProgressInfo {
                Message = !string.IsNullOrWhiteSpace(message)
                    ? !parameters.IsNullOrEmpty()
                        ? string.Format(message, parameters)
                        : message
                    : string.Format(Strings.Progress_PerformStep, actualStep, totalSteps),
                ActualStep = actualStep,
                TotalSteps = totalSteps,
                State = ProgressState.PerformStep
            });
        }

        public static void Complete(this IProgress<ProgressInfo> source, int actualStep = 0, int totalSteps = 0, string message = null, params object[] parameters) {
            if (source == null) { return; }

            source.Report(new ProgressInfo {
                Message = !string.IsNullOrWhiteSpace(message)
                    ? !parameters.IsNullOrEmpty()
                        ? string.Format(message, parameters)
                        : message
                    : Strings.Progress_Complete,
                ActualStep = actualStep,
                TotalSteps = totalSteps,
                State = ProgressState.Complete
            });
        }

        public static void Cancel(this IProgress<ProgressInfo> source, int actualStep = 0, int totalSteps = 0, string message = null, params object[] parameters) {
            if (source == null) { return; }

            source.Report(new ProgressInfo {
                Message = !string.IsNullOrWhiteSpace(message)
                    ? !parameters.IsNullOrEmpty()
                        ? string.Format(message, parameters)
                        : message
                    : Strings.Progress_Cancel,
                ActualStep = actualStep,
                TotalSteps = totalSteps,
                State = ProgressState.Cancel
            });
        }

        public static void Error(this IProgress<ProgressInfo> source, int actualStep = 0, int totalSteps = 0, string message = null, params object[] parameters) {
            if (source == null) { return; }

            source.Report(new ProgressInfo {
                Message = !string.IsNullOrWhiteSpace(message)
                    ? !parameters.IsNullOrEmpty()
                        ? string.Format(message, parameters)
                        : message
                    : Strings.Progress_Error,
                ActualStep = actualStep,
                TotalSteps = totalSteps,
                State = ProgressState.Error
            });
        }

        #endregion Public Static Methods
    }
}