-- Create new table for document directories.
CREATE TABLE IF NOT EXISTS [document_directories] (
    [document_directory_id] INTEGER PRIMARY KEY AUTOINCREMENT,
    [code]                  TEXT                NOT NULL,
    [label]                 TEXT                NOT NULL,
    [path]                  TEXT                NOT NULL,
    [position]              INTEGER             NOT NULL,

    CONSTRAINT [UQ_document_directory_path] UNIQUE ([path])
);

-- Copy data from the old document directories table.
INSERT INTO [document_directories] (
    [document_directory_id],
    [code],
    [label],
    [path],
    [position]
)
SELECT
    [id],
    [code],
    [label],
    [path],
    0
FROM [old_document_directories];

-- Create new table for documents.
CREATE TABLE IF NOT EXISTS [documents] (
    [document_id]           INTEGER PRIMARY KEY AUTOINCREMENT,
    [document_directory_id] INTEGER             NOT NULL,
    [code]                  INTEGER             NOT NULL,
    [content]               TEXT                NOT NULL,
    [payload]               BLOB                NULL,
    [path]                  TEXT                NOT NULL,
    [last_write_time]       DATETIME            NOT NULL,
    [index]                 INTEGER             NOT NULL,

    CONSTRAINT [UQ_document_path] UNIQUE ([path]),

    FOREIGN KEY([document_directory_id]) REFERENCES [document_directories]([document_directory_id]) ON DELETE CASCADE
);

-- Copy data from old documents table.
INSERT INTO [documents] (
    [document_id],
    [document_directory_id],
    [code],
    [content],
    [payload],
    [path],
    [last_write_time],
    [index]
)
SELECT
    [id],
    [document_directory_id],
    [code],
    [content],
    [payload],
    [path],
    [last_write_time],
    [indexed]
FROM [old_documents];