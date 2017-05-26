using System;
using System.Collections.Generic;
using System.IO;
using Lucene.Net.Util;

namespace InfoFenix.Core.Search {
    /// <summary>
    /// Default implementation of <see cref="IIndexProvider"/>
    /// </summary>
    public class IndexProvider : IIndexProvider {

        #region Private Static Read-Only Fields

        private static readonly IDictionary<string, IIndex> Cache = new Dictionary<string, IIndex>(StringComparer.InvariantCultureIgnoreCase);
        private static readonly object SyncLock = new object();

        #endregion Private Static Read-Only Fields

        #region Private Read-Only Fields

        private readonly IAnalyzerProvider _analyzerProvider;
        private readonly LuceneSettings _settings;

        #endregion Private Read-Only Fields

        #region Public Static Read-Only Fields

        /// <summary>
        /// Gets the Lucene version used.
        /// </summary>
        public static readonly LuceneVersion Version = LuceneVersion.LUCENE_48;

        #endregion Public Static Read-Only Fields

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="IndexProvider"/>.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="hostingEnvironment">The hosting environment.</param>
        /// <param name="analyzerProvider">The analyzer provider.</param>
        public IndexProvider(IAnalyzerProvider analyzerProvider, LuceneSettings settings) {
            if (analyzerProvider == null) {
                throw new ArgumentNullException(nameof(analyzerProvider));
            }
            if (settings == null) {
                throw new ArgumentNullException(nameof(settings));
            }

            _analyzerProvider = analyzerProvider;
            _settings = settings;
        }

        #endregion Public Constructors

        #region Private Methods

        private IIndex InnerCreate(string indexName) {
            return new Index(_analyzerProvider.GetAnalyzer(indexName), _settings.IndexStorageDirectoryPath, indexName);
        }

        #endregion Private Methods

        #region IIndexProvider Members

        public void Delete(string indexName) {
            lock (SyncLock) {
                if (!Cache.ContainsKey(indexName)) { return; }

                Directory.Delete(Path.Combine(_settings.IndexStorageDirectoryPath, indexName));
                if (Cache[indexName] is IDisposable disposable) {
                    disposable.Dispose();
                }
                Cache.Remove(indexName);
            }
        }

        public bool Exists(string indexName) {
            return Cache.ContainsKey(indexName);
        }

        public IIndex GetOrCreate(string indexName) {
            lock (SyncLock) {
                if (!Cache.ContainsKey(indexName)) {
                    Cache.Add(indexName, InnerCreate(indexName));
                }

                return Cache[indexName];
            }
        }

        public IEnumerable<string> List() {
            return Cache.Keys;
        }

        #endregion IIndexProvider Members
    }
}