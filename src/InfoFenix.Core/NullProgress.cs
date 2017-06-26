using System;

namespace InfoFenix.Core {

    /// <summary>
    /// A null object pattern implementation of <see cref="IProgress{T}"/>.
    /// </summary>
    public sealed class NullProgress<T> : IProgress<T> {

        #region Private Static Fields

        private static readonly IProgress<T> CurrentInstance = new NullProgress<T>();

        #endregion Private Static Fields

        #region Public Static Properties

        /// <summary>
        /// Gets the current single instance of <see cref="NullProgress"/>.
        /// </summary>
        public static IProgress<T> Instance {
            get { return CurrentInstance; }
        }

        #endregion Public Static Properties

        #region IProgress<T> Members

        public void Report(T value) {
        }

        #endregion IProgress<T> Members
    }
}