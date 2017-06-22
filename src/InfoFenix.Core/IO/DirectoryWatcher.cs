using System;
using System.IO;
using InfoFenix.Core.PubSub;

namespace InfoFenix.Core.IO {
    public sealed class DirectoryWatcher : IDirectoryWatcher, IDisposable {

        #region Private Read-Only Fields

        private readonly IPublisherSubscriber _publisherSubscriber;

        #endregion Private Read-Only Fields

        #region Private Fields

        private string _path;
        private FileSystemWatcher _fileSystemWatcher;
        private bool _disposed;

        #endregion Private Fields

        #region Public Constructors

        public DirectoryWatcher(IPublisherSubscriber publisherSubscriber) {
            Prevent.ParameterNull(publisherSubscriber, nameof(publisherSubscriber));

            _publisherSubscriber = publisherSubscriber;
        }

        #endregion Public Constructors

        #region Destructor

        ~DirectoryWatcher() {
            Dispose(disposing: false);
        }

        #endregion Destructor

        #region Private Methods

        private void DirectoryContentChange(object sender, FileSystemEventArgs e) {
            if (!IOHelper.IsFile(e.FullPath)) { return; }

            // Ignore temporary files.
            if (Path.GetFileName(e.FullPath).StartsWith("~")) { return; }
            // Ignore error files.
            if (!File.Exists(e.FullPath)) { return; }

            _publisherSubscriber.Publish(new DirectoryContentChangeNotification {
                WatchingPath = _path,
                FullPath = e.FullPath,
                Changes = (DirectoryContentChangeNotification.ChangeTypes)(int)e.ChangeType
            });
        }

        private void Dispose(bool disposing) {
            if (_disposed) { return; }
            if (disposing) {
                if (_fileSystemWatcher != null) {
                    _fileSystemWatcher.EnableRaisingEvents = false;
                    _fileSystemWatcher.Dispose();
                }
            }
            _fileSystemWatcher = null;
            _disposed = true;
        }

        #endregion Private Methods

        #region IDirectoryWatcher Members

        public void Watch(string path) {
            if (!IOHelper.IsDirectory(path)) { return; }

            _path = path;
            _fileSystemWatcher = new FileSystemWatcher {
                EnableRaisingEvents = false,
                Filter = "*.*",
                IncludeSubdirectories = true,
                NotifyFilter = NotifyFilters.CreationTime | NotifyFilters.LastWrite | NotifyFilters.FileName,
                Path = path
            };

            _fileSystemWatcher.Created += DirectoryContentChange;
            _fileSystemWatcher.Changed += DirectoryContentChange;
            _fileSystemWatcher.Deleted += DirectoryContentChange;

            _fileSystemWatcher.EnableRaisingEvents = true;
        }

        #endregion IDirectoryWatcher Members

        #region IDisposable Members

        public void Dispose() {
            Dispose(disposing: true);
            GC.SuppressFinalize(obj: this);
        }

        #endregion IDisposable Members
    }
}