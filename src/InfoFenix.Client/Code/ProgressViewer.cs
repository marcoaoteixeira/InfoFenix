using System;
using System.Diagnostics;
using System.Threading;
using InfoFenix.Client.Views.Shared;
using InfoFenix.Core;
using InfoFenix.Core.Cqrs;
using InfoFenix.Core.Logging;

namespace InfoFenix.Client.Code {

    public static class ProgressViewer {

        #region Public Static Methods

        public static void Display(CancellationTokenIssuer cancellationTokenIssuer, ILogger log = null, params Action<CancellationToken, IProgress<ProgressInfo>>[] actions) {
            log = log ?? NullLogger.Instance;

            using (var form = new ProgressViewerForm(cancellationTokenIssuer)) {
                var stopwatch = Stopwatch.StartNew();
                form.Run(actions);
                stopwatch.Stop();
                log.Information($"ProgressViewer running time: {stopwatch.ElapsedMilliseconds} ms ({stopwatch.ElapsedMilliseconds / 1000} sec.)");

                form.ShowDialog();
            }
        }

        #endregion Public Static Methods
    }
}