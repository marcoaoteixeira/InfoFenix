SELECT
    [documents].[document_id],
    [documents].[document_directory_id],
    [documents].[code],
    [documents].[path],
    [documents].[last_write_time],
    [documents].[index]
FROM [documents]
    INNER JOIN [document_directories] ON [document_directories].[document_directory_id] = [documents].[document_directory_id]
WHERE
    [document_directories].[document_directory_id] = @document_directory_id;