﻿using System;
using System.IO;
using System.Linq;
using Lucene.Net.Analysis;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using LuceneDirectory = Lucene.Net.Store.Directory;

namespace InfoFenix.Core.Search {
    public class Index : IIndex, IDisposable {

        #region Private Read-Only Fields

        private readonly Analyzer _analyzer;
        private readonly string _basePath;

        #endregion Private Read-Only Fields

        #region Private Fields

        private LuceneDirectory _directory;
        private IndexReader _indexReader;
        private IndexSearcher _indexSearcher;
        private IndexWriter _indexWriter;
        private bool _disposed;

        #endregion Private Fields

        #region Public Static Read-Only Fields

        /// <summary>
        /// GEts the default minimun date time.
        /// </summary>
        public static readonly DateTime DefaultMinDateTime = new DateTime(1980, 1, 1);

        /// <summary>
        /// Gets the batch size.
        /// </summary>
        public static readonly int BatchSize = BooleanQuery.MaxClauseCount;

        #endregion Public Static Read-Only Fields

        #region Public Constructors

        public Index(Analyzer analyzer, string basePath, string name) {
            Prevent.ParameterNull(analyzer, nameof(analyzer));
            Prevent.ParameterNullOrWhiteSpace(basePath, nameof(basePath));
            Prevent.ParameterNullOrWhiteSpace(name, nameof(name));

            _analyzer = analyzer;
            _basePath = basePath;

            Name = name;

            Initialize();
        }

        #endregion Public Constructors

        #region Destructor

        ~Index() {
            Dispose(disposing: false);
        }

        #endregion Destructor

        #region Private Static Methods

        private static Document CreateDocument(IDocumentIndex documentIndex) {
            var documentIndexImpl = documentIndex as DocumentIndex;
            if (documentIndexImpl == null) {
                throw new InvalidCastException($"Parameter {nameof(documentIndex)} must be of type {nameof(DocumentIndex)}");
            }

            var document = new Document();
            documentIndexImpl.PrepareForIndexing();
            foreach (var field in documentIndexImpl.Fields) {
                document.Add(field);
            }
            return document;
        }

        #endregion Private Static Methods

        #region Private Methods

        private void Initialize() {
            _directory = Lucene.Net.Store.FSDirectory.Open(new DirectoryInfo(Path.Combine(_basePath, Name)));
            _indexReader = DirectoryReader.Open(_directory);
            _indexSearcher = new IndexSearcher(_indexReader);
            _indexWriter = new IndexWriter(_directory, new IndexWriterConfig(IndexProvider.Version, _analyzer));
        }

        private bool IndexDirectoryExists() {
            return Directory.Exists(Path.Combine(_basePath, Name));
        }

        private void Dispose(bool disposing) {
            if (_disposed) { return; }
            if (disposing) {
                if (_directory != null) {
                    _directory.Dispose();
                }
                if (_indexReader != null) {
                    _indexReader.Dispose();
                }
                if (_indexWriter != null) {
                    _indexWriter.Dispose();
                }
            }

            _directory = null;
            _indexReader = null;
            _indexSearcher = null;
            _indexWriter = null;
            _disposed = true;
        }

        #endregion Private Methods

        #region IIndex Members

        public string Name { get; }

        public bool IsEmpty() {
            return TotalDocuments() <= 0;
        }

        public int TotalDocuments() {
            if (!IndexDirectoryExists()) { return -1; }

            return _indexReader.NumDocs;
        }

        public IDocumentIndex NewDocument(string documentID) {
            return new DocumentIndex(documentID);
        }

        public void StoreDocuments(params IDocumentIndex[] documents) {
            if (documents == null) { return; }
            if (documents.Length == 0) { return; }

            DeleteDocuments(documents.OfType<DocumentIndex>().Select(_ => _.DocumentID).ToArray());

            foreach (var document in documents) {
                _indexWriter.AddDocument(CreateDocument(document));
            }
        }

        public void DeleteDocuments(params string[] documentIDs) {
            if (documentIDs == null) { return; }
            if (documentIDs.Length == 0) { return; }

            // Process documents by batch as there is a max number of terms a query can contain (1024 by default).

            var pageCount = documentIDs.Length / (BatchSize + 1);
            for (var page = 0; page < pageCount; page++) {
                var query = new BooleanQuery();
                try {
                    var batch = documentIDs.Skip(page * BatchSize).Take(BatchSize);
                    foreach (var id in batch) {
                        query.Add(new BooleanClause(new TermQuery(new Term(nameof(SearchHit.DocumentID), id)), BooleanClause.Occur.SHOULD));
                    }
                    _indexWriter.DeleteDocuments(query);
                } catch (Exception) { /* Just skip error */ }
            }
        }

        public ISearchBuilder CreateSearchBuilder() {
            return new SearchBuilder(_analyzer, _indexSearcher);
        }

        #endregion IIndex Members

        #region IDisposable Members

        public void Dispose() {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable Members
    }
}