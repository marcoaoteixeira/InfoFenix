CREATE TABLE IF NOT EXISTS [document_directories] (
    [document_directory_id] INTEGER PRIMARY KEY AUTOINCREMENT,
    [label]                 TEXT                NOT NULL,
    [directory_path]        TEXT                NOT NULL,
    [code]                  TEXT                NOT NULL,
    [watch]                 INTEGER             NOT NULL,
    [index]                 INTEGER             NOT NULL,

    CONSTRAINT [UQ_directory_path] UNIQUE ([directory_path])
);

CREATE TABLE IF NOT EXISTS [documents] (
    [document_id]               INTEGER PRIMARY KEY AUTOINCREMENT,
    [document_directory_id]     INTEGER             NOT NULL,
    [full_path]                 TEXT                NOT NULL,
    [last_write_time]           DATETIME            NOT NULL,
    [code]                      INTEGER             NOT NULL,
    [indexed]                   INTEGER             NOT NULL,
    [payload]                   BLOB                NULL,

    FOREIGN KEY([document_directory_id]) REFERENCES [document_directories]([document_directory_id])
);