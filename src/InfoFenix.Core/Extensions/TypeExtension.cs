using System;
using System.Reflection;

namespace InfoFenix.Core {
    public static class TypeExtension {

        #region Public Static Methods

        public static TAttribute GetCustomAttribute<TAttribute>(this Type source, TAttribute fallback)
            where TAttribute : Attribute {
            if (source == null) { return fallback; }

            var attribute = source.GetCustomAttribute<TAttribute>();

            return attribute != null ? attribute : fallback;
        }

        #endregion Public Static Methods
    }
}