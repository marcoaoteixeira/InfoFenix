INSERT OR REPLACE INTO [document_directories] (
    [document_directory_id],
    [code],
    [label],
    [path],
    [position]
) VALUES (
    @document_directory_id,
    @code,
    @label,
    @path,
    @position
);
SELECT MAX([document_directory_id]) FROM [document_directories]