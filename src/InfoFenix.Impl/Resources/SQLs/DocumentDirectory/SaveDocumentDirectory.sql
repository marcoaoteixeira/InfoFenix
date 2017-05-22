INSERT OR REPLACE INTO [document_directories] (
    [document_directory_id],
    [label],
    [directory_path],
    [code],
    [watch],
    [index]
) VALUES (
    @DocumentDirectoryID,
    @Label,
    @DirectoryPath,
    @Code,
    @Watch,
    @Index
);
SELECT MAX([document_directory_id]) FROM [document_directories]