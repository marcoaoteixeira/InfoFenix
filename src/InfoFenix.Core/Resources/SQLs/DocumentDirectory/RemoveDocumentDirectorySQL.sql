DELETE FROM [documents]
WHERE
    [document_directory_id] = @document_directory_id;

DELETE FROM [document_directories]
WHERE
    [id] = @document_directory_id;