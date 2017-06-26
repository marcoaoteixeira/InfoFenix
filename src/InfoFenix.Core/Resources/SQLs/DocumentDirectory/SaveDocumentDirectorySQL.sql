INSERT OR REPLACE INTO [document_directories] (
    [id],
    [label],
    [path],
    [code],
    [watch],
    [index]
) VALUES (
    @id,
    @label,
    @path,
    @code,
    @watch,
    @index
);
SELECT MAX([id]) FROM [document_directories]