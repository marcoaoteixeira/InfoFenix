SELECT
    [document_directories].[id],
    [document_directories].[label],
    [document_directories].[path],
    [document_directories].[code],
    [document_directories].[watch],
    [document_directories].[index],
    (SELECT
        COUNT([documents].[id])
     FROM [documents]
     WHERE
        [documents].[document_directory_id] = [document_directories].[id]
    ) AS [total_documents]
FROM [document_directories]
WHERE
    [document_directories].[id] = @id;