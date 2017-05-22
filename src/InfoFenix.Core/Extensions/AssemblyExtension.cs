using System;
using System.IO;
using System.Reflection;

namespace InfoFenix.Core {

    public static class AssemblyExtension {

        #region Public Static Methods

        public static string GetDirectoryPath(this Assembly source) {
            if (source == null) { return null; }

            var codeBase = source.CodeBase;
            var uri = new UriBuilder(codeBase);
            var path = Uri.UnescapeDataString(uri.Path);

            return Path.GetDirectoryName(path);
        }

        #endregion Public Static Methods
    }
}