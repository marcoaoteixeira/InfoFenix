DELETE FROM [documents]
WHERE
    [document_directory_id] = @DocumentDirectoryID;

DELETE FROM [document_directories]
WHERE
    [id] = @DocumentDirectoryID;