using System;
using System.Runtime.Serialization;

namespace InfoFenix.Core {

    [Serializable]
    public class FatalException : Exception {

        public FatalException() {
        }

        public FatalException(string message)
            : base(message) {
        }

        public FatalException(string message, Exception inner)
            : base(message, inner) {
        }

        protected FatalException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}