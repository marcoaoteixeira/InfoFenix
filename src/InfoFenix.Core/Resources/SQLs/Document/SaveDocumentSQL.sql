INSERT OR REPLACE INTO [documents] (
    [id],
    [document_directory_id],
    [path],
    [last_write_time],
    [code],
    [indexed],
    [content],
    [payload]
) VALUES (
    @id,
    @document_directory_id,
    @path,
    @last_write_time,
    @code,
    @indexed,
    @content,
    @payload
);
SELECT MAX([id]) FROM [documents];