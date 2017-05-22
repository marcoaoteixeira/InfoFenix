SELECT
    [documents].[document_id],
    [documents].[document_directory_id],
    [documents].[file_name],
    [documents].[full_path],
    [documents].[last_write_time],
    [documents].[code],
    [documents].[indexed],
    [documents].[payload]
FROM [documents]
    INNER JOIN [document_directories] ON [document_directories].[document_directory_id] = [documents].[document_directory_id]
WHERE
    [document_directories].[document_directory_id] = @DocumentDirectoryID;