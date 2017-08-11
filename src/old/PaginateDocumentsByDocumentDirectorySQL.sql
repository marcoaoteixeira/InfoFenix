SELECT
    [documents].[document_id],
    [documents].[document_directory_id],
    [documents].[code],
    [documents].[content],
    [documents].[payload],
    [documents].[path],
    [documents].[last_write_time],
    [documents].[index]
FROM [documents]
    INNER JOIN [document_directories] ON [document_directories].[document_directory_id] = [documents].[document_directory_id]
WHERE
    [document_directories].[document_directory_id] = @document_directory_id
AND (@must_index IS NULL OR ([documents].[index] = @must_index))
ORDER BY
    [documents].[document_id]
LIMIT @skip, @count