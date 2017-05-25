INSERT OR REPLACE INTO [documents] (
    [id],
    [document_directory_id],
    [path],
    [last_write_time],
    [code],
    [indexed],
    [payload]
) VALUES (
    @ID,
    @DocumentDirectoryID,
    @Path,
    @LastWriteTime,
    @Code,
    @Indexed,
    @Payload
);
SELECT MAX([id]) FROM [documents];