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
    (@label IS NULL OR ([document_directories].[label] LIKE '%' + @label + '%'))
AND (@path IS NULL OR ([document_directories].[path] LIKE '%' + @path + '%'))
AND (@code IS NULL OR ([document_directories].[code] LIKE '%' + @code + '%'))
AND (@watch IS NULL OR ([document_directories].[watch] = @watch))
AND (@index IS NULL OR ([document_directories].[index] = @index));