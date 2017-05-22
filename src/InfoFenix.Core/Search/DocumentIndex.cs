using System;
using System.Collections.Generic;
using Lucene.Net.Documents;
using Lucene.Net.Index;

namespace InfoFenix.Core.Search {

    /// <summary>
    /// Default implementation of <see cref="IDocumentIndex"/>.
    /// </summary>
    public class DocumentIndex : IDocumentIndex {

        #region Private Read-Only Fields

        private readonly IList<IndexableField> _fields = new List<IndexableField>();

        #endregion Private Read-Only Fields

        #region Private Fields

        private string _name;
        private string _stringValue;
        private int _intValue;
        private double _doubleValue;
        private bool _analyze;
        private bool _store;
        private bool _removeHtmlTags;
        private TypeCode _typeCode;

        #endregion Private Fields

        #region Public Properties

        /// <summary>
        /// Gets all fields.
        /// </summary>
        public IEnumerable<IndexableField> Fields {
            get { return _fields; }
        }

        /// <summary>
        /// Gets the content item ID.
        /// </summary>
        public string DocumentID { get; private set; }

        #endregion Public Properties

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="DocumentIndex"/>
        /// </summary>
        /// <param name="documentID">The document ID.</param>
        public DocumentIndex(string documentID) {
            SetDocumentID(documentID);
            IsDirty = false;
            _typeCode = TypeCode.Empty;
        }

        #endregion Public Constructors

        #region Internal Methods

        internal void PrepareForIndexing() {
            switch (_typeCode) {
                case TypeCode.String:
                    if (_removeHtmlTags) {
                        _stringValue = _stringValue.RemoveHtmlTags(htmlDecode: true);
                    }
                    if (_analyze) {
                        _fields.Add(new TextField(
                            name: _name,
                            value: _stringValue,
                            store: _store ? Field.Store.YES : Field.Store.NO));
                    } else {
                        _fields.Add(new StringField(
                            name: _name,
                            value: _stringValue,
                            stored: _store ? Field.Store.YES : Field.Store.NO));
                    }
                    break;

                case TypeCode.Int32:
                    _fields.Add(new IntField(
                        name: _name,
                        value: _intValue,
                        stored: _store ? Field.Store.YES : Field.Store.NO));
                    break;

                case TypeCode.Single:
                case TypeCode.Double:
                    _fields.Add(new DoubleField(
                        name: _name,
                        value: _doubleValue,
                        stored: _store ? Field.Store.YES : Field.Store.NO));
                    break;

                case TypeCode.Empty:
                    break;

                default:
                    throw new InvalidOperationException("Unexpected index type");
            }

            _analyze = false;
            _removeHtmlTags = false;
            _store = false;
            _typeCode = TypeCode.Empty;
        }

        #endregion Internal Methods

        #region IDocumentIndex Members

        /// <inheritdoc />
        public bool IsDirty { get; private set; }

        /// <inheritdoc />
        public IDocumentIndex SetDocumentID(string id) {
            DocumentID = id;

            var field = new StringField(
                name: nameof(ISearchHit.DocumentID),
                value: id,
                stored: Field.Store.YES);

            _fields.Add(field);

            return this;
        }

        /// <inheritdoc />
        public IDocumentIndex Add(string name, string value) {
            PrepareForIndexing();

            _name = name;
            _stringValue = value ?? string.Empty;
            _typeCode = TypeCode.String;

            IsDirty = true;

            return this;
        }

        /// <inheritdoc />
        public IDocumentIndex Add(string name, DateTime value) {
            return Add(name, DateTools.DateToString(value, DateTools.Resolution.MILLISECOND));
        }

        /// <inheritdoc />
        public IDocumentIndex Add(string name, int value) {
            PrepareForIndexing();

            _name = name;
            _intValue = value;
            _typeCode = TypeCode.Int32;

            IsDirty = true;

            return this;
        }

        /// <inheritdoc />
        public IDocumentIndex Add(string name, bool value) {
            return Add(name, value ? 1 : 0);
        }

        /// <inheritdoc />
        public IDocumentIndex Add(string name, double value) {
            PrepareForIndexing();

            _name = name;
            _doubleValue = value;
            _typeCode = TypeCode.Double;

            IsDirty = true;

            return this;
        }

        /// <inheritdoc />
        public IDocumentIndex Add(string name, object value) {
            return Add(name, (value != null ? value.ToString() : string.Empty));
        }

        /// <inheritdoc />
        public IDocumentIndex Analyze() {
            _analyze = true;

            return this;
        }

        /// <inheritdoc />
        public IDocumentIndex Store() {
            _store = true;

            return this;
        }

        /// <inheritdoc />
        public IDocumentIndex RemoveHtmlTags() {
            _removeHtmlTags = true;

            return this;
        }

        #endregion IDocumentIndex Members
    }
}