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
    (@code IS NULL OR ([document_directories].[code] LIKE '%' + @code + '%'))
AND (@label IS NULL OR ([document_directories].[label] LIKE '%' + @label + '%'))
AND (@path IS NULL OR ([document_directories].[path] LIKE '%' + @path + '%'));