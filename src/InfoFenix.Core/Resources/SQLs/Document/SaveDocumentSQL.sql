INSERT OR REPLACE INTO [documents] (
    [id],
    [document_directory_id],
    [path],
    [last_write_time],
    [code],
    [indexed],
    [payload]
) VALUES (
    @id,
    @document_directory_id,
    @path,
    @last_write_time,
    @code,
    @indexed,
    @payload
);
SELECT MAX([id]) FROM [documents];