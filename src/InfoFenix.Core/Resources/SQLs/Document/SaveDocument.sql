INSERT OR REPLACE INTO [documents] (
    [document_id],
    [document_directory_id],
    [full_path],
    [last_write_time],
    [code],
    [indexed],
    [payload]
) VALUES (
    @DocumentID,
    @DocumentDirectoryID,
    @FullPath,
    @LastWriteTime,
    @Code,
    @Indexed,
    @Payload
);
SELECT MAX([document_id]) FROM [documents];