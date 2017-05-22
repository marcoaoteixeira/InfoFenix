﻿using Lucene.Net.Analysis;

namespace InfoFenix.Core.Search {
    /// <summary>
    /// Defines methods to a Lucene analyzer provider.
    /// </summary>
    public interface IAnalyzerProvider {

        #region Methods

        /// <summary>
        /// Retrieves the analyzer.
        /// </summary>
        /// <param name="indexName">The index name.</param>
        /// <returns>An instance of <see cref="Analyzer"/>.</returns>
        Analyzer GetAnalyzer(string indexName);

        #endregion Methods
    }
}