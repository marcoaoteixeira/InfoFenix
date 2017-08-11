﻿CREATE TABLE IF NOT EXISTS [document_directories] (
    [document_directory_id] INTEGER PRIMARY KEY AUTOINCREMENT,
    [code]                  TEXT                NOT NULL,
    [label]                 TEXT                NOT NULL,
    [path]                  TEXT                NOT NULL,
    [position]              INTEGER             NOT NULL,

    CONSTRAINT [UQ_document_directory_path] UNIQUE ([path])
);

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

CREATE INDEX IF NOT EXISTS [IDX_documents_code] ON [documents] ([code] ASC);