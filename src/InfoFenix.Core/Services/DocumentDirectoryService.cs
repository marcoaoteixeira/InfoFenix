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
            _publisherSubscriber.PublishAsync(new ProgressiveTaskStartNotification {
                Title = "Salvar Diretório de Documentos",
                Message = $"Salvando diretório de documentos ({documentDirectory.Label}) na base de dados...",
                TotalSteps = 1
            });
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

            _publisherSubscriber.PublishAsync(new ProgressiveTaskPerformStepNotification {
                Title = "Salvar Diretório de Documentos",
                Message = $"Diretório de documentos ({documentDirectory.Label}) salvo com sucesso.",
                ActualStep = 1,
                TotalSteps = 1
            });
            _publisherSubscriber.PublishAsync(new ProgressiveTaskCompleteNotification {
                Title = "Salvar Diretório de Documentos",
                Message = $"Diretório de documentos ({documentDirectory.Label}) salvo com sucesso.",
                TotalSteps = 1
            });
        }

        public Task SaveDocumentsInsideDocumentDirectoryAsync(int documentDirectoryID, CancellationToken cancellationToken) {
            var documentDirectory = Get(documentDirectoryID);
            if (documentDirectory == null) { return Task.FromResult(0); }

            var physicalFiles = Common.GetDocFiles(documentDirectory.Path);
            var databaseDocuments = _commandQueryDispatcher
                .Query(new ListDocumentsByDocumentDirectoryQuery { DocumentDirectoryID = documentDirectory.ID })
                .ToArray();

            _publisherSubscriber.PublishAsync(new ProgressiveTaskStartNotification {
                Title = "Salvar Documentos do Diretório de Documentos",
                Message = $"Salvando documentos do diretório de documentos ({documentDirectory.Label}) na base de dados...",
                TotalSteps = physicalFiles.Length
            });

            var counter = 1;
            foreach (var physicalFile in physicalFiles) {
                if (cancellationToken.IsCancellationRequested) {
                    _publisherSubscriber.PublishAsync(new ProgressiveTaskCompleteNotification {
                        Title = "Salvar Documentos do Diretório de Documentos",
                        Message = $"Tarefa cancelada",
                        TotalSteps = counter
                    });

                    return Task.FromResult(0);
                }

                // Document não está na base de dados? Adiciona.
                var document = databaseDocuments.SingleOrDefault(_ => string.Equals(_.Path, physicalFile, StringComparison.InvariantCultureIgnoreCase));
                SaveDocumentCommand command = null;
                if (document == null) {
                    command = new SaveDocumentCommand {
                        DocumentDirectoryID = documentDirectory.ID,
                        Path = physicalFile,
                        LastWriteTime = File.GetLastWriteTime(physicalFile),
                        Code = Common.ExtractCodeFromFilePath(physicalFile),
                        Indexed = false,
                        Payload = File.ReadAllBytes(physicalFile)
                    };
                }
                // Caso exista, verifica a data do arquivo com a data do documento, se for diferente, atualiza.
                if (document != null && document.LastWriteTime != File.GetLastWriteTime(physicalFile)) {
                    command = new SaveDocumentCommand {
                        ID = document.ID,
                        DocumentDirectoryID = document.DocumentDirectoryID,
                        Path = document.Path,
                        LastWriteTime = File.GetLastWriteTime(physicalFile) /* UPDATE */,
                        Code = document.Code,
                        Indexed = false,
                        Payload = File.ReadAllBytes(physicalFile) /* UPDATE */
                    };
                }
                if (command != null) {
                    _commandQueryDispatcher.Command(command);
                }

                _publisherSubscriber.PublishAsync(new ProgressiveTaskPerformStepNotification {
                    Title = "Salvar Documentos do Diretório de Documentos",
                    Message = $"Salvando documento: {Path.GetFileName(physicalFile)}",
                    ActualStep = counter++,
                    TotalSteps = physicalFiles.Length
                });
            }

            _publisherSubscriber.PublishAsync(new ProgressiveTaskCompleteNotification {
                Title = "Salvar Documentos do Diretório de Documentos",
                Message = $"Documentos do diretório de documentos ({documentDirectory.Label}) salvos com sucesso.",
                TotalSteps = physicalFiles.Length
            });

            return Task.FromResult(0);
        }

        public void Remove(int documentDirectoryID) {
            var documentDirectory = Get(documentDirectoryID);
            if (documentDirectory == null) { return; }

            _publisherSubscriber.PublishAsync(new ProgressiveTaskStartNotification {
                Title = "Remover Diretório de Documentos",
                Message = $"Removendo diretório de documentos ({documentDirectory.Label}) da base de dados...",
                TotalSteps = 1
            });

            _commandQueryDispatcher.Command(new RemoveDocumentDirectoryCommand {
                ID = documentDirectory.ID,
                Label = documentDirectory.Label,
                Code = documentDirectory.Code
            });

            _publisherSubscriber.PublishAsync(new ProgressiveTaskPerformStepNotification {
                Title = "Remover Diretório de Documentos",
                Message = $"Diretório de documentos ({documentDirectory.Label}) removido com sucesso.",
                ActualStep = 1,
                TotalSteps = 1
            });
            _publisherSubscriber.PublishAsync(new ProgressiveTaskCompleteNotification {
                Title = "Remover Diretório de Documentos",
                Message = $"Diretório de documentos ({documentDirectory.Label}) removido com sucesso.",
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

        public Task IndexAsync(int documentDirectoryID, CancellationToken cancellationToken) {
            const int BATCH_SIZE = 128;

            var documentDirectory = Get(documentDirectoryID);
            if (documentDirectory == null) { return Task.FromResult(0); }

            var documents = _commandQueryDispatcher
                .Query(new ListDocumentsByDocumentDirectoryQuery { DocumentDirectoryID = documentDirectoryID })
                .ToArray();

            var index = _indexProvider.GetOrCreate(documentDirectory.Code);

            _publisherSubscriber.PublishAsync(new ProgressiveTaskStartNotification {
                Title = "Indexar Diretório de Documentos",
                Message = $"Indexando diretório de documentos ({documentDirectory.Label})...",
                TotalSteps = documents.Length
            });

            var documentsToIndex = new List<IDocumentIndex>();
            var counter = 1;
            foreach (var document in documents) {
                if (cancellationToken.IsCancellationRequested) {
                    // Index the actual documents.
                    index.StoreDocuments(documentsToIndex.ToArray());

                    _publisherSubscriber.PublishAsync(new ProgressiveTaskCompleteNotification {
                        Title = "Indexar Diretório de Documentos",
                        Message = $"Tarefa cancelada.",
                        TotalSteps = counter
                    });

                    return Task.FromResult(0);
                }

                try {
                    string content = string.Empty;
                    using (var memoryStream = new MemoryStream(document.Payload))
                    using (var wordDocument = _wordApplication.Open(memoryStream)) {
                        content = wordDocument.Text;
                    }
                    
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
                        Title = "Indexar Diretório de Documentos",
                        Message = $"Documento: ({document.FileName})",
                        ActualStep = counter,
                        TotalSteps = documents.Length
                    });
                } catch (Exception ex) {
                    Log.Error(ex, $"ERROR INDEXING FILE: {document.Path}");

                    _publisherSubscriber.PublishAsync(new ProgressiveTaskPerformStepNotification {
                        Title = "Indexar Diretório de Documentos",
                        Message = $"Erro ao indexar o arquivo: ({document.FileName})",
                        ActualStep = counter,
                        TotalSteps = documents.Length
                    });
                } finally { counter++; }
            }

            // Index the current documents.
            index.StoreDocuments(documentsToIndex.ToArray());

            _publisherSubscriber.PublishAsync(new ProgressiveTaskCompleteNotification {
                Title = "Indexar Diretório de Documentos",
                Message = $"Diretório de documentos ({documentDirectory.Label}) indexado com sucesso.",
                TotalSteps = documents.Length
            });

            return Task.FromResult(0);
        }

        public void WatchForModification(int documentDirectoryID) {
            var documentDirectory = Get(documentDirectoryID);
            if (documentDirectory == null) { return; }

            _publisherSubscriber.PublishAsync(new ProgressiveTaskStartNotification {
                Title = "Observar Diretório de Documentos",
                Message = $"Iniciando observação do diretório de documentos ({documentDirectory.Label})...",
                TotalSteps = 1
            });

            _commandQueryDispatcher.Command(new StartWatchDocumentDirectoryCommand {
                DirectoryPath = documentDirectory.Path
            });

            _publisherSubscriber.PublishAsync(new ProgressiveTaskPerformStepNotification {
                Title = "Observar Diretório de Documentos",
                Message = $"Observando diretório de documentos ({documentDirectory.Label}).",
                ActualStep = 1,
                TotalSteps = 1
            });
            _publisherSubscriber.PublishAsync(new ProgressiveTaskCompleteNotification {
                Title = "Observar Diretório de Documentos",
                Message = $"Diretório de documentos ({documentDirectory.Label}) sendo observado com sucesso.",
                TotalSteps = 1
            });
        }

        public async Task CleanAsync(int documentDirectoryID, CancellationToken cancellationToken) {
            var documentDirectory = Get(documentDirectoryID);
            if (documentDirectory == null) { return; }

            var physicalFiles = Common.GetDocFiles(documentDirectory.Path);
            var databaseDocuments = _commandQueryDispatcher
                .Query(new ListDocumentsByDocumentDirectoryQuery { DocumentDirectoryID = documentDirectory.ID })
                .ToArray();
            var toRemove = databaseDocuments
                .Where(_ => !physicalFiles.Contains(_.Path, StringComparer.InvariantCultureIgnoreCase))
                .ToArray();

            await _publisherSubscriber.PublishAsync(new ProgressiveTaskStartNotification {
                Title = "Limpar Diretório de Documentos",
                Message = $"Iniciando limpeza do diretório de documentos ({documentDirectory.Label})...",
                TotalSteps = 1
            });

            var counter = 1;
            foreach (var document in toRemove) {
                if (cancellationToken.IsCancellationRequested) {
                    await _publisherSubscriber.PublishAsync(new ProgressiveTaskCompleteNotification {
                        Title = "Limpar Diretório de Documentos",
                        Message = $"Tarefa cancelada.",
                        TotalSteps = counter
                    });

                    return;
                }

                _commandQueryDispatcher.Command(new RemoveDocumentCommand {
                    ID = document.ID,
                    FileName = Path.GetFileNameWithoutExtension(document.Path),
                    DocumentDirectoryCode = documentDirectory.Code
                });

                await _publisherSubscriber.PublishAsync(new ProgressiveTaskPerformStepNotification {
                    Title = "Limpar Diretório de Documentos",
                    Message = $"Documento removido: ({documentDirectory.Label})",
                    ActualStep = counter++,
                    TotalSteps = toRemove.Length
                });
            }
            await _publisherSubscriber.PublishAsync(new ProgressiveTaskCompleteNotification {
                Title = "Limpar Diretório de Documentos",
                Message = $"Diretório de documentos ({documentDirectory.Label}) limpo com sucesso.",
                TotalSteps = toRemove.Length
            });
        }

        #endregion IDocumentDirectoryService Members
    }
}