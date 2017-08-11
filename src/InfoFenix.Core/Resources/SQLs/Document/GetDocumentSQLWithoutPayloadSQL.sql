SELECT
    [document_id],
    [document_directory_id],
    [code],
    [content],
    [path],
    [last_write_time],
    [index]
FROM [documents]
WHERE
    (@document_id IS NULL OR ([document_id] = @document_id))
AND (@code IS NULL OR ([code] = @code))