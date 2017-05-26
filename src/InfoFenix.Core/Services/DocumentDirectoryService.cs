using System.Collections.Generic;
using InfoFenix.Core.Commands;
using InfoFenix.Core.Cqrs;
using InfoFenix.Core.Entities;
using InfoFenix.Core.Logging;
using InfoFenix.Core.PubSub;
using InfoFenix.Core.Queries;
using System.Linq;
using System;
using System.IO;

namespace InfoFenix.Core.Services {

    public sealed class DocumentDirectoryService : IDocumentDirectoryService {

        #region Private Read-Only Fields

        private readonly ICqrsDispatcher _cqrsDispatcher;
        private readonly IPublisherSubscriber _publisherSubscriber;

        #endregion Private Read-Only Fields

        #region Public Properties

        private ILogger _log;

        public ILogger Log {
            get { return _log ?? NullLogger.Instance; }
            set { _log = value ?? NullLogger.Instance; }
        }

        #endregion Public Properties

        #region Public Constructors

        public DocumentDirectoryService(ICqrsDispatcher cqrsDispatcher, IPublisherSubscriber publisherSubscriber) {
            Prevent.ParameterNull(cqrsDispatcher, nameof(cqrsDispatcher));
            Prevent.ParameterNull(publisherSubscriber, nameof(publisherSubscriber));

            _cqrsDispatcher = cqrsDispatcher;
            _publisherSubscriber = publisherSubscriber;
        }

        #endregion Public Constructors

        #region IDocumentDirectoryService Members

        public void Save(DocumentDirectoryEntity documentDirectory, bool saveDocumentsInsideDirectory = false) {
            var command = new SaveDocumentDirectoryCommand {
                ID = documentDirectory.ID,
                Label = documentDirectory.Label,
                Path = documentDirectory.Path,
                Code = documentDirectory.Code,
                Index = documentDirectory.Index,
                Watch = documentDirectory.Watch
            };

            _cqrsDispatcher.Command(command);
            if (documentDirectory.ID <= 0) { documentDirectory.ID = command.ID; }
        }

        public void Remove(int documentDirectoryID) {
            var documentDirectory = Get(documentDirectoryID);
            if (documentDirectory == null) { return; }

            _cqrsDispatcher.Command(new RemoveDocumentDirectoryCommand {
                ID = documentDirectory.ID,
                Label = documentDirectory.Label,
                Code = documentDirectory.Code
            });
        }

        public DocumentDirectoryEntity Get(int documentDirectoryID) {
            var documentDirectory = _cqrsDispatcher.Query(new GetDocumentDirectoryQuery { ID = documentDirectoryID });
            if (documentDirectory == null) {
                Log.Information($"Não foi possível localizar o diretório de documentos na base de dados. (ID: {documentDirectoryID})");
            }
            return documentDirectory;
        }

        public IEnumerable<DocumentDirectoryEntity> List(string label = null, string path = null, string code = null) {
            return _cqrsDispatcher.Query(new ListDocumentDirectoriesQuery {
                Label = label,
                Path = path,
                Code = code
            });
        }

        public void Index(int documentDirectoryID, bool reCreate = false) {
            var documentDirectory = Get(documentDirectoryID);
            if (documentDirectory == null) { return; }

            var checkedDocuments = new List<string>(); /* DOCUMENTS PATH */
            var documents = _cqrsDispatcher
                .Query(new ListDocumentsByDocumentDirectoryQuery { DocumentDirectoryID = documentDirectoryID })
                .ToArray();
            var physicalFiles = Common.GetDocFiles(documentDirectory.Path);

            foreach (var physicalFile in physicalFiles) {

                // Document não está na base de dados? Adiciona.
                var document = documents.Single(_ => string.Equals(_.Path, physicalFile, StringComparison.InvariantCultureIgnoreCase));
                if (document == null) {
                    _cqrsDispatcher.Command(new SaveDocumentCommand {
                        DocumentDirectoryID = documentDirectoryID,
                        Path = physicalFile,
                        LastWriteTime = File.GetLastWriteTime(physicalFile),
                        Code = Common.ExtractCodeFromFilePath(physicalFile),
                        Indexed = false,
                        Payload = File.ReadAllBytes(physicalFile)
                    });
                }

                if (document != null && document.LastWriteTime != File.GetLastWriteTime(physicalFile)) {
                    _cqrsDispatcher.Command(new SaveDocumentCommand {
                        ID = document.ID,
                        DocumentDirectoryID = document.DocumentDirectoryID,
                        Path = document.Path,
                        LastWriteTime = File.GetLastWriteTime(physicalFile) /* UPDATE */,
                        Code = document.Code,
                        Indexed = false,
                        Payload = File.ReadAllBytes(physicalFile) /* UPDATE */
                    });
                    checkedDocuments.Add(document.Path);
                }
            }
        }

        public void WatchForModification(int documentDirectoryID) {
            var documentDirectory = Get(documentDirectoryID);
            if (documentDirectory == null) { return; }
        }

        #endregion IDocumentDirectoryService Members
    }
}