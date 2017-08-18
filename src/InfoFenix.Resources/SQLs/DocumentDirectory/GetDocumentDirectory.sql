SELECT
    [document_directories].[document_directory_id],
    [document_directories].[code],
    [document_directories].[label],
    [document_directories].[path],
    [document_directories].[position],
    (SELECT
        COUNT([documents].[document_id])
     FROM [documents]
     WHERE
        [documents].[document_directory_id] = [document_directories].[document_directory_id]
    ) AS [total_documents]
FROM [document_directories]
WHERE
    [document_directories].[document_directory_id] = @document_directory_id;