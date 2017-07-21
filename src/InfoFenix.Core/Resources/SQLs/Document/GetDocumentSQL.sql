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
    (@id IS NULL OR ([id] = @id))
AND (@code IS NULL OR ([code] = @code))