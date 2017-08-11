SELECT
    [document_id],
    [document_directory_id],
    [code],
    [content],
    [payload],
    [path],
    [last_write_time],
    [index]
FROM [documents]
WHERE
    [path] = @path