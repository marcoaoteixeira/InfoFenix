SELECT
    [id],
    [document_directory_id],
    [path],
    [last_write_time],
    [code],
    [indexed],
    [content],
    [payload]
FROM [documents]
WHERE
    [path] = @path