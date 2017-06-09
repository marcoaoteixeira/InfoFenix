SELECT
    [document_directories].[id],
    [document_directories].[label],
    [document_directories].[path],
    [document_directories].[code],
    [document_directories].[watch],
    [document_directories].[index],
    COUNT([documents].[id]) AS [total_documents]
FROM [document_directories]
INNER JOIN [documents] ON [documents].[document_directory_id] = [document_directories].[id]
WHERE
    [document_directories].[id] = @ID;