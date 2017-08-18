using System;
using System.Runtime.Serialization;

namespace InfoFenix {

    [Serializable]
    public class FatalException : Exception {

        #region Public Constructors

        public FatalException() {
        }

        public FatalException(string message)
            : base(message) { }

        public FatalException(string message, Exception inner)
            : base(message, inner) { }

        #endregion Public Constructors

        #region Protected Constructors

        protected FatalException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }

        #endregion Protected Constructors
    }
}