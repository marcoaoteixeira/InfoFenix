using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InfoFenix.Core.Commands;
using InfoFenix.Core.Cqrs;
using InfoFenix.Core.Entities;
using InfoFenix.Core.Logging;
using InfoFenix.Core.Office;
using InfoFenix.Core.PubSub;
using InfoFenix.Core.Queries;
using InfoFenix.Core.Search;

namespace InfoFenix.Core.Services {

    public sealed class DocumentDirectoryService : IDocumentDirectoryService {

        #region Private Read-Only Fields

        private readonly ICommandQueryDispatcher _commandQueryDispatcher;
        private readonly IIndexProvider _indexProvider;
        private readonly IPublisherSubscriber _publisherSubscriber;
        private readonly IWordApplication _wordApplication;

        #endregion Private Read-Only Fields

        #region Public Properties

        private ILogger _log;

        public ILogger Log {
            get { return _log ?? NullLogger.Instance; }
            set { _log = value ?? NullLogger.Instance; }
        }

        #endregion Public Properties

        #region Public Constructors

        public DocumentDirectoryService(ICommandQueryDispatcher commandQueryDispatcher, IIndexProvider indexProvider, IPublisherSubscriber publisherSubscriber, IWordApplication wordApplication) {
            Prevent.ParameterNull(commandQueryDispatcher, nameof(commandQueryDispatcher));
            Prevent.ParameterNull(indexProvider, nameof(indexProvider));
            Prevent.ParameterNull(publisherSubscriber, nameof(publisherSubscriber));
            Prevent.ParameterNull(wordApplication, nameof(wordApplication));

            _commandQueryDispatcher = commandQueryDispatcher;
            _indexProvider = indexProvider;
            _publisherSubscriber = publisherSubscriber;
            _wordApplication = wordApplication;
        }

        #endregion Public Constructors

        #region IDocumentDirectoryService Members

        public void Save(DocumentDirectoryEntity documentDirectory) {
            const string notificationTitle = "Salvar Diretório de Documentos";

            _publisherSubscriber.PublishAsync(new ProgressiveTaskStartNotification {
                Title = notificationTitle,
                TotalSteps = 1
            });

            try {
                var command = new SaveDocumentDirectoryCommand {
                    ID = documentDirectory.ID,
                    Label = documentDirectory.Label,
                    Path = documentDirectory.Path,
                    Code = documentDirectory.Code,
                    Index = documentDirectory.Index,
                    Watch = documentDirectory.Watch
                };
                _commandQueryDispatcher.Command(command);
                if (documentDirectory.ID <= 0) { documentDirectory.ID = command.ID; }
            } catch {
                _publisherSubscriber.PublishAsync(new ProgressiveTaskCriticalErrorNotification {
                    Title = notificationTitle,
                });

                throw;
            }

            _publisherSubscriber.PublishAsync(new ProgressiveTaskPerformStepNotification {
                Title = notificationTitle,
                Message = $"Salvando o diretório de documentos ({documentDirectory.Label})...",
                ActualStep = 1,
                TotalSteps = 1
            });

            _publisherSubscriber.PublishAsync(new ProgressiveTaskCompleteNotification {
                Title = notificationTitle,
                TotalSteps = 1
            });
        }

        public Task SaveDocumentsInsideDocumentDirectoryAsync(int documentDirectoryID, CancellationToken cancellationToken) {
            const string notificationTitle = "Salvar Documentos do Diretório de Documentos";

            var documentDirectory = Get(documentDirectoryID);
            if (documentDirectory == null) { return Task.FromResult(0); }

            var physicalFiles = Common.GetDocFiles(documentDirectory.Path);
            var databaseDocuments = _commandQueryDispatcher
                .Query(new ListDocumentsByDocumentDirectoryQuery { DocumentDirectoryID = documentDirectory.ID })
                .ToArray();

            _publisherSubscriber.PublishAsync(new ProgressiveTaskStartNotification {
                Title = notificationTitle,
                Message = "Iniciando tarefa...",
                TotalSteps = physicalFiles.Length
            });

            var databaseFiles = databaseDocuments.Select(_ => _.Path).ToArray();
            var toCreate = physicalFiles.Except(databaseFiles);
            var counter = 1;
            foreach (var file in toCreate) {
                if (cancellationToken.IsCancellationRequested) {
                    _publisherSubscriber.PublishAsync(new ProgressiveTaskCancelNotification {
                        Title = notificationTitle,
                        TotalSteps = counter
                    });

                    return Task.FromResult(0);
                }

                // Document não está na base de dados? Adiciona.
                var document = databaseDocuments.SingleOrDefault(_ => string.Equals(_.Path, file, StringComparison.InvariantCultureIgnoreCase));
                SaveDocumentCommand command = null;
                if (document == null) {
                    command = new SaveDocumentCommand {
                        DocumentDirectoryID = documentDirectory.ID,
                        Path = file,
                        LastWriteTime = File.GetLastWriteTime(file),
                        Code = Common.ExtractCodeFromFilePath(file),
                        Indexed = false,
                        Payload = File.ReadAllBytes(file)
                    };
                }
                // Caso exista, verifica a data do arquivo com a data do documento, se for diferente, atualiza.
                if (document != null && document.LastWriteTime != File.GetLastWriteTime(file)) {
                    command = new SaveDocumentCommand {
                        ID = document.ID,
                        DocumentDirectoryID = document.DocumentDirectoryID,
                        Path = document.Path,
                        LastWriteTime = File.GetLastWriteTime(file) /* UPDATE */,
                        Code = document.Code,
                        Indexed = false,
                        Payload = File.ReadAllBytes(file) /* UPDATE */
                    };
                }

                try { if (command != null) { _commandQueryDispatcher.Command(command); } } catch { Log.Error($"ERRO AO SALVAR DOCUMENTO. CAMINHO: {file}"); }

                _publisherSubscriber.PublishAsync(new ProgressiveTaskPerformStepNotification {
                    Title = notificationTitle,
                    Message = $"Documento: {Path.GetFileName(file)}",
                    ActualStep = counter++,
                    TotalSteps = physicalFiles.Length
                });
            }

            _publisherSubscriber.PublishAsync(new ProgressiveTaskCompleteNotification {
                Title = notificationTitle,
                Message = $"Documentos do diretório de documentos ({documentDirectory.Label}) salvos com sucesso.",
                TotalSteps = physicalFiles.Length
            });

            return Task.FromResult(0);
        }

        public void Remove(int documentDirectoryID) {
            var documentDirectory = Get(documentDirectoryID);
            if (documentDirectory == null) { return; }

            const string notificationTitle = "Remover Diretório de Documentos";

            _publisherSubscriber.PublishAsync(new ProgressiveTaskStartNotification {
                Title = notificationTitle,
                TotalSteps = 1
            });

            _commandQueryDispatcher.Command(new RemoveDocumentDirectoryCommand {
                ID = documentDirectory.ID,
                Label = documentDirectory.Label,
                Code = documentDirectory.Code
            });

            _publisherSubscriber.PublishAsync(new ProgressiveTaskPerformStepNotification {
                Title = notificationTitle,
                Message = $"Diretório de documentos ({documentDirectory.Label}) removido com sucesso.",
                ActualStep = 1,
                TotalSteps = 1
            });
            _publisherSubscriber.PublishAsync(new ProgressiveTaskCompleteNotification {
                Title = notificationTitle,
                TotalSteps = 1
            });
        }

        public DocumentDirectoryEntity Get(int documentDirectoryID) {
            var documentDirectory = _commandQueryDispatcher.Query(new GetDocumentDirectoryQuery { ID = documentDirectoryID });
            if (documentDirectory == null) {
                Log.Information($"Não foi possível localizar o diretório de documentos na base de dados. (ID: {documentDirectoryID})");
            }
            return documentDirectory;
        }

        public IEnumerable<DocumentDirectoryEntity> List(string label = null, string path = null, string code = null) {
            return _commandQueryDispatcher.Query(new ListDocumentDirectoriesQuery {
                Label = label,
                Path = path,
                Code = code
            });
        }

        public IEnumerable<DocumentEntity> GetDocuments(int documentDirectoryID) {
            return _commandQueryDispatcher.Query(new ListDocumentsByDocumentDirectoryQuery {
                DocumentDirectoryID = documentDirectoryID
            });
        }

        public Task IndexAsync(int documentDirectoryID, CancellationToken cancellationToken) {
            const int BATCH_SIZE = 128;
            const string notificationTitle = "Indexar Diretório de Documentos";

            var documentDirectory = Get(documentDirectoryID);
            if (documentDirectory == null) { return Task.FromResult(0); }

            var documents = _commandQueryDispatcher
                .Query(new ListDocumentsByDocumentDirectoryQuery { DocumentDirectoryID = documentDirectoryID })
                .Where(_ => !_.Indexed)
                .ToArray();

            var index = _indexProvider.GetOrCreate(documentDirectory.Code);

            _publisherSubscriber.PublishAsync(new ProgressiveTaskStartNotification {
                Title = notificationTitle,
                TotalSteps = documents.Length
            });

            var documentsToIndex = new List<IDocumentIndex>();
            var counter = 1;
            foreach (var document in documents) {
                if (cancellationToken.IsCancellationRequested) {
                    // Index the actual documents.
                    index.StoreDocuments(documentsToIndex.ToArray());

                    _publisherSubscriber.PublishAsync(new ProgressiveTaskCancelNotification {
                        Title = notificationTitle,
                        TotalSteps = counter
                    });

                    return Task.FromResult(0);
                }

                string content = string.Empty;
                try {
                    using (var memoryStream = new MemoryStream(document.Payload))
                    using (var wordDocument = _wordApplication.Open(memoryStream)) {
                        content = wordDocument.Text;
                    }
                } catch (Exception ex) {
                    Log.Error(ex, $"ERROR OPENING DOCUMENT: {document.Path}");

                    _publisherSubscriber.PublishAsync(new ProgressiveTaskPerformStepNotification {
                        Title = notificationTitle,
                        Message = $"Erro ao indexar o arquivo: ({document.FileName})",
                        ActualStep = counter,
                        TotalSteps = documents.Length
                    });

                    continue;
                }

                try {
                    var documentIndex = index.NewDocument(document.ID.ToString());
                    documentIndex.Add(Common.Index.DocumentFieldName.Content, content).Analyze();
                    documentIndex.Add(Common.Index.DocumentFieldName.DocumentDirectoryCode, documentDirectory.Code).Store();
                    documentIndex.Add(Common.Index.DocumentFieldName.DocumentCode, document.Code).Store();
                    documentsToIndex.Add(documentIndex);

                    _commandQueryDispatcher.Command(new SetDocumentIndexCommand { ID = document.ID });

                    // Flush documents if needed.
                    if (counter % BATCH_SIZE == 0) {
                        index.StoreDocuments(documentsToIndex.ToArray());
                        documentsToIndex.Clear();
                    }

                    _publisherSubscriber.PublishAsync(new ProgressiveTaskPerformStepNotification {
                        Title = notificationTitle,
                        Message = $"Documento: ({document.FileName})",
                        ActualStep = counter,
                        TotalSteps = documents.Length
                    });
                } catch (Exception ex) {
                    Log.Error(ex, $"ERROR INDEXING FILE: {document.Path}");

                    _publisherSubscriber.PublishAsync(new ProgressiveTaskPerformStepNotification {
                        Title = notificationTitle,
                        Message = $"Erro ao indexar o arquivo: ({document.FileName})",
                        ActualStep = counter,
                        TotalSteps = documents.Length
                    });
                } finally { counter++; }
            }

            // Index the current documents.
            index.StoreDocuments(documentsToIndex.ToArray());

            _publisherSubscriber.PublishAsync(new ProgressiveTaskCompleteNotification {
                Title = notificationTitle,
                TotalSteps = documents.Length
            });

            return Task.FromResult(0);
        }

        public void StartWatchForModification(int documentDirectoryID) {
            var documentDirectory = Get(documentDirectoryID);
            if (documentDirectory == null) { return; }

            const string notificationTitle = "Observar Diretório de Documentos";

            _publisherSubscriber.PublishAsync(new ProgressiveTaskStartNotification {
                Title = notificationTitle,
                Message = $"Iniciando observação do diretório de documentos ({documentDirectory.Label})...",
                TotalSteps = 1
            });

            _commandQueryDispatcher.Command(new StartWatchDocumentDirectoryCommand {
                DirectoryPath = documentDirectory.Path
            });

            _publisherSubscriber.PublishAsync(new ProgressiveTaskPerformStepNotification {
                Title = notificationTitle,
                Message = $"Observando diretório de documentos ({documentDirectory.Label}).",
                ActualStep = 1,
                TotalSteps = 1
            });
            _publisherSubscriber.PublishAsync(new ProgressiveTaskCompleteNotification {
                Title = notificationTitle,
                Message = $"Diretório de documentos ({documentDirectory.Label}) sendo observado com sucesso.",
                TotalSteps = 1
            });
        }

        public void StopWatchForModification(int documentDirectoryID) {
            var documentDirectory = Get(documentDirectoryID);
            if (documentDirectory == null) { return; }

            const string notificationTitle = "Observar Diretório de Documentos";

            _publisherSubscriber.PublishAsync(new ProgressiveTaskStartNotification {
                Title = notificationTitle,
                Message = $"Parando observação do diretório de documentos ({documentDirectory.Label})...",
                TotalSteps = 1
            });

            _commandQueryDispatcher.Command(new StopWatchDocumentDirectoryCommand {
                DirectoryPath = documentDirectory.Path
            });

            _publisherSubscriber.PublishAsync(new ProgressiveTaskPerformStepNotification {
                Title = notificationTitle,
                Message = $"Executando ações necessários...",
                ActualStep = 1,
                TotalSteps = 1
            });
            _publisherSubscriber.PublishAsync(new ProgressiveTaskCompleteNotification {
                Title = notificationTitle,
                Message = $"O diretório de documentos ({documentDirectory.Label}) não está mais sendo observado.",
                TotalSteps = 1
            });
        }

        public Task CleanAsync(int documentDirectoryID, CancellationToken cancellationToken) {
            const string notificationTitle = "Limpar Diretório de Documentos";

            var documentDirectory = Get(documentDirectoryID);
            if (documentDirectory == null) { return Task.FromResult(0); }

            var physicalFiles = Common.GetDocFiles(documentDirectory.Path);
            var databaseDocuments = _commandQueryDispatcher
                .Query(new ListDocumentsByDocumentDirectoryQuery { DocumentDirectoryID = documentDirectory.ID })
                .ToArray();
            var toRemove = databaseDocuments
                .Where(_ => !physicalFiles.Contains(_.Path, StringComparer.InvariantCultureIgnoreCase))
                .ToArray();

            _publisherSubscriber.PublishAsync(new ProgressiveTaskStartNotification {
                Title = notificationTitle,
                Message = $"Iniciando limpeza do diretório de documentos ({documentDirectory.Label})...",
                TotalSteps = toRemove.Length
            });

            var counter = 1;
            foreach (var document in toRemove) {
                if (cancellationToken.IsCancellationRequested) {
                    _publisherSubscriber.PublishAsync(new ProgressiveTaskCompleteNotification {
                        Title = notificationTitle,
                        TotalSteps = counter
                    });

                    return Task.FromResult(0);
                }

                _commandQueryDispatcher.Command(new RemoveDocumentCommand {
                    ID = document.ID,
                    FileName = Path.GetFileNameWithoutExtension(document.Path),
                    DocumentDirectoryCode = documentDirectory.Code
                });

                _publisherSubscriber.PublishAsync(new ProgressiveTaskPerformStepNotification {
                    Title = notificationTitle,
                    Message = $"Documento: {document.FileName}",
                    ActualStep = counter++,
                    TotalSteps = toRemove.Length
                });
            }
            _publisherSubscriber.PublishAsync(new ProgressiveTaskCompleteNotification {
                Title = notificationTitle,
                Message = $"Diretório de documentos ({documentDirectory.Label}) limpo com sucesso.",
                TotalSteps = toRemove.Length
            });

            return Task.FromResult(0);
        }

        #endregion IDocumentDirectoryService Members
    }
}