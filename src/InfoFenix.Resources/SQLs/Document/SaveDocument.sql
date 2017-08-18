INSERT OR REPLACE INTO [documents] (
    [document_id],
    [document_directory_id],
    [code],
    [content],
    [payload],
    [path],
    [last_write_time],
    [index]
) VALUES (
    @document_id,
    @document_directory_id,
    @code,
    @content,
    @payload,
    @path,
    @last_write_time,
    @index
);
SELECT MAX([document_id]) FROM [documents];