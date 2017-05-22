using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoFenix.Core.PubSub {
    public sealed class DirectoryContentChangeNotification {
        #region Public Properties

        public string WatchingPath { get; set; }
        public string FullPath { get; set; }
        public ChangeTypes Changes { get; set; }

        #endregion

        #region Public Enums

        [Flags]
        public enum ChangeTypes {
            Created = 1,
            Deleted = 2,
            Changed = 4,
            Renamed = 8,
            All = 15
        }

        #endregion
    }
}
