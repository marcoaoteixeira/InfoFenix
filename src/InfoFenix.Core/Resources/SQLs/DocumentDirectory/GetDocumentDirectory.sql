SELECT
    [id],
    [label],
    [path],
    [code],
    [watch],
    [index]
FROM [document_directories]
WHERE
    [id] = @ID;