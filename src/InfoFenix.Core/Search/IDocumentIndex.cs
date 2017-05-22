using System;

namespace InfoFenix.Core.Search {
    /// <summary>
    /// Defines methods for a document index.
    /// </summary>
    public interface IDocumentIndex {

        #region Properties

        /// <summary>
        /// Whether some property have been added to this document, or otherwise if it's empty
        /// </summary>
        bool IsDirty { get; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Sets the document ID.
        /// </summary>
        /// <param name="documentID">Document ID.</param>
        /// <returns>The current instance of <see cref="IDocumentIndex"/>.</returns>
        IDocumentIndex SetDocumentID(string documentID);

        /// <summary>
        /// Adds a new <see cref="string"/> value to the document index.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <returns>The current instance of <see cref="IDocumentIndex"/>.</returns>
        IDocumentIndex Add(string name, string value);

        /// <summary>
        /// Adds a new <see cref="DateTime"/> value to the document index.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <returns>The current instance of <see cref="IDocumentIndex"/>.</returns>
        IDocumentIndex Add(string name, DateTime value);

        /// <summary>
        /// Adds a new <see cref="int"/> value to the document index.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <returns>The current instance of <see cref="IDocumentIndex"/>.</returns>
        IDocumentIndex Add(string name, int value);

        /// <summary>
        /// Adds a new <see cref="bool"/> value to the document index.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <returns>The current instance of <see cref="IDocumentIndex"/>.</returns>
        IDocumentIndex Add(string name, bool value);

        /// <summary>
        /// Adds a new <see cref="double"/> value to the document index.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <returns>The current instance of <see cref="IDocumentIndex"/>.</returns>
        IDocumentIndex Add(string name, double value);

        /// <summary>
        /// Document is analyzed and tokenized.
        /// </summary>
        IDocumentIndex Analyze();

        /// <summary>
        /// Stores the original value to the index.
        /// </summary>
        /// <returns>The current instance of <see cref="IDocumentIndex"/>.</returns>
        IDocumentIndex Store();

        /// <summary>
        /// Remove any HTML tag from the current string
        /// </summary>
        /// <returns>The current instance of <see cref="IDocumentIndex"/>.</returns>
        IDocumentIndex RemoveHtmlTags();

        #endregion Methods
    }
}