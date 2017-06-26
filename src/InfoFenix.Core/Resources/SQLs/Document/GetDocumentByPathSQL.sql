SELECT
    [id],
    [document_directory_id],
    [path],
    [last_write_time],
    [code],
    [indexed],
    [payload]
FROM [documents]
WHERE
    [path] = @path