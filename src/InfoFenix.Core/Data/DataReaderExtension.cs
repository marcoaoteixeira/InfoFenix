using System;
using System.Data;

namespace InfoFenix.Core.Data {

    /// <summary>
    /// Extension methods for <see cref="IDataReader"/>
    /// </summary>
    public static class DataReaderExtension {

        #region Public Static Methods

        public static Guid GetGuidOrDefault(this IDataReader source, string fieldName, Guid fallback = default(Guid)) {
            Prevent.ParameterNullOrWhiteSpace(fieldName, nameof(fieldName));

            if (source == null) { return fallback; }

            var value = source[fieldName];

            return (value != null && value != DBNull.Value) ? Guid.Parse(value.ToString()) : fallback;
        }

        public static string GetStringOrDefault(this IDataReader source, string fieldName, string fallback = null) {
            Prevent.ParameterNullOrWhiteSpace(fieldName, nameof(fieldName));

            if (source == null) { return fallback; }

            var value = source[fieldName];

            return (value != null && value != DBNull.Value) ? value.ToString() : fallback;
        }

        public static int GetInt32OrDefault(this IDataReader source, string fieldName, int fallback = 0) {
            Prevent.ParameterNullOrWhiteSpace(fieldName, nameof(fieldName));

            if (source == null) { return fallback; }

            var value = source[fieldName];

            return (value != null && value != DBNull.Value) ? Convert.ToInt32(value) : fallback;
        }

        public static bool GetBooleanOrDefault(this IDataReader source, string fieldName, bool fallback = false) {
            Prevent.ParameterNullOrWhiteSpace(fieldName, nameof(fieldName));

            if (source == null) { return fallback; }

            var value = source[fieldName];

            return (value != null && value != DBNull.Value) ? Convert.ToBoolean(value) : fallback;
        }

        public static DateTime GetDateTimeOrDefault(this IDataReader source, string fieldName, DateTime fallback) {
            Prevent.ParameterNullOrWhiteSpace(fieldName, nameof(fieldName));

            if (source == null) { return fallback; }

            var value = source[fieldName];

            return (value != null && value != DBNull.Value) ? Convert.ToDateTime(value) : fallback;
        }

        public static DateTime? GetDateTimeOrDefault(this IDataReader source, string fieldName) {
            Prevent.ParameterNullOrWhiteSpace(fieldName, nameof(fieldName));

            var value = source[fieldName];

            return (value != null && value != DBNull.Value) ? Convert.ToDateTime(value) : new DateTime?();
        }

        public static DateTimeOffset GetDateTimeOffsetOrDefault(this IDataReader source, string fieldName, DateTimeOffset fallback) {
            Prevent.ParameterNullOrWhiteSpace(fieldName, nameof(fieldName));

            if (source == null) { return fallback; }

            var value = source[fieldName];

            return (value != null && value != DBNull.Value) ? DateTimeOffset.Parse(value.ToString()) : fallback;
        }

        public static DateTimeOffset? GetDateTimeOffsetOrDefault(this IDataReader source, string fieldName) {
            Prevent.ParameterNullOrWhiteSpace(fieldName, nameof(fieldName));

            var value = source[fieldName];

            return (value != null && value != DBNull.Value) ? DateTimeOffset.Parse(value.ToString()) : new DateTimeOffset?();
        }

        public static byte[] GetBlobOrDefault(this IDataReader source, string fieldName, byte[] fallback = null) {
            Prevent.ParameterNullOrWhiteSpace(fieldName, nameof(fieldName));

            if (source == null) { return fallback; }

            var value = source[fieldName];

            return (value != null && value != DBNull.Value) ? (byte[])value : fallback;
        }

        #endregion Public Static Methods
    }
}