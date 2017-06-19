﻿using System.Collections.Generic;
using System.Data;
using InfoFenix.Core.Data;

namespace InfoFenix.Core.Dto {

    public class DocumentDirectoryDto {

        #region Public Properties

        public int DocumentDirectoryID { get; set; }
        public string Label { get; set; }
        public string Path { get; set; }
        public string Code { get; set; }
        public bool Watch { get; set; }
        public bool Index { get; set; }

        public IList<DocumentDto> Documents { get; set; } = new List<DocumentDto>();

        #endregion Public Properties

        #region Public Static Methods

        public static DocumentDirectoryDto Map(IDataReader reader) {
            return new DocumentDirectoryDto {
                DocumentDirectoryID = reader.GetInt32OrDefault(Common.DatabaseSchema.DocumentDirectories.DocumentDirectoryID),
                Label = reader.GetStringOrDefault(Common.DatabaseSchema.DocumentDirectories.Label),
                Path = reader.GetStringOrDefault(Common.DatabaseSchema.DocumentDirectories.Path),
                Code = reader.GetStringOrDefault(Common.DatabaseSchema.DocumentDirectories.Code),
                Watch = reader.GetInt32OrDefault(Common.DatabaseSchema.DocumentDirectories.Watch) > 0,
                Index = reader.GetInt32OrDefault(Common.DatabaseSchema.DocumentDirectories.Index) > 0
            };
        }

        #endregion Public Static Methods
    }
}