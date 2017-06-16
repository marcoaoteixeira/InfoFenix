CREATE TABLE IF NOT EXISTS [document_directories] (
    [id]    INTEGER PRIMARY KEY AUTOINCREMENT,
    [label] TEXT                NOT NULL,
    [path]  TEXT                NOT NULL,
    [code]  TEXT                NOT NULL,
    [watch] INTEGER             NOT NULL,
    [index] INTEGER             NOT NULL,

    CONSTRAINT [UQ_document_directory_path] UNIQUE ([path])
);

CREATE TABLE IF NOT EXISTS [documents] (
    [id]                    INTEGER PRIMARY KEY AUTOINCREMENT,
    [document_directory_id] INTEGER             NOT NULL,
    [path]                  TEXT                NOT NULL,
    [last_write_time]       DATETIME            NOT NULL,
    [code]                  INTEGER             NOT NULL,
    [indexed]               INTEGER             NOT NULL,
    [payload]               BLOB                NULL,

    CONSTRAINT [UQ_document_path] UNIQUE ([path]),

    FOREIGN KEY([document_directory_id]) REFERENCES [document_directories]([id]) ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS [IDX_documents_code] ON [documents] ([code] ASC);