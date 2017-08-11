﻿using System;
using System.Threading;
using InfoFenix.Client.Views.Shared;
using InfoFenix.Core;
using InfoFenix.Core.CQRS;
using InfoFenix.Core.Logging;

namespace InfoFenix.Client.Code {

    public static class ProgressViewer {

        #region Public Static Methods

        public static void Display(CancellationTokenIssuer cancellationTokenIssuer, ILogger log = null, params Action<CancellationToken, IProgress<ProgressInfo>>[] actions) {
            log = log ?? NullLogger.Instance;

            using (var form = new ProgressViewerForm(cancellationTokenIssuer)) {
                form.Run(actions);
                form.ShowDialog();
            }
        }

        #endregion Public Static Methods
    }
}