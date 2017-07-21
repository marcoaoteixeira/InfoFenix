DELETE FROM [documents]
WHERE
    [document_directory_id] = @id;

DELETE FROM [document_directories]
WHERE
    [id] = @id;