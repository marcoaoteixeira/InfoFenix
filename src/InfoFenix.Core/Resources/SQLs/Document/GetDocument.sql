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
    (@ID IS NULL OR ([id] = @ID))
AND (@Code IS NULL OR ([code] = @Code))