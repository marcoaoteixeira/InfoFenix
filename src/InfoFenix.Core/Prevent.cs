using System;

namespace InfoFenix.Core {
    public static class Prevent {

        #region Public Static Methods

        public static void ParameterNull(object parameterValue, string parameterName) {
            CheckParameterName(parameterName);

            if (parameterValue == null) {
                throw new ArgumentNullException(parameterName);
            }
        }

        public static void ParameterNullOrWhiteSpace(string parameterValue, string parameterName) {
            CheckParameterName(parameterName);

            if (string.IsNullOrWhiteSpace(parameterValue)) {
                throw new ArgumentException("Parameter cannot be null, empty or white spaces.", nameof(parameterName));
            }
        }

        #endregion Public Static Methods

        #region Private Static Methods

        private static void CheckParameterName(string parameterName) {
            if (string.IsNullOrWhiteSpace(parameterName)) {
                throw new ArgumentException("Parameter cannot be null, empty or white spaces.", nameof(parameterName));
            }
        }

        #endregion Private Static Methods
    }
}