using System.IO;

namespace InfoFenix.Core {

    public static class StreamExtension {

        #region Public Static Methods

        public static byte[] ToArray(this Stream source) {
            if (source == null) { return null; }

            if (source is MemoryStream) { return ((MemoryStream)source).ToArray(); }

            var buffer = new byte[8 * 1024 /* 8 Kb */];
            using (var memoryStream = new MemoryStream()) {
                var count = 0;
                while ((count = source.Read(buffer, 0, buffer.Length)) > 0) {
                    memoryStream.Write(buffer, 0, count);
                }

                return memoryStream.ToArray();
            }
        }

        #endregion Public Static Methods
    }
}