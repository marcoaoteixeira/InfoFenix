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
    (@Label IS NULL OR ([document_directories].[label] LIKE '%' + @Label + '%'))
AND (@Path IS NULL OR ([document_directories].[path] LIKE '%' + @Path + '%'))
AND (@Code IS NULL OR ([document_directories].[code] LIKE '%' + @Code + '%'))
AND (@Watch IS NULL OR ([document_directories].[watch] = @Watch))
AND (@Index IS NULL OR ([document_directories].[index] = @Index));