SELECT
    [documents].[id],
    [documents].[document_directory_id],
    [documents].[path],
    [documents].[last_write_time],
    [documents].[code],
    [documents].[indexed],
    [documents].[payload]
FROM [documents]
    INNER JOIN [document_directories] ON [document_directories].[id] = [documents].[document_directory_id]
WHERE
    [document_directories].[id] = @document_directory_id;