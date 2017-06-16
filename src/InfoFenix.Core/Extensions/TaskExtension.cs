using System.Threading.Tasks;

namespace InfoFenix.Core {

    public static class TaskExtension {

        #region Public Static Methods

        public static TResult WaitForResult<TResult>(this Task<TResult> source) {
            if (source == null) { return default(TResult); }

            source.Wait();

            return source.Result;
        }

        public static void WaitForResult(this Task source) {
            if (source == null) { return; }

            source.Wait();
        }

        #endregion Public Static Methods
    }
}