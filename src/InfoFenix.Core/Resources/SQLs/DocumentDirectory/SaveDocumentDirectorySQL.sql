INSERT OR REPLACE INTO [document_directories] (
    [id],
    [label],
    [path],
    [code],
    [watch],
    [index]
) VALUES (
    @ID,
    @Label,
    @Path,
    @Code,
    @Watch,
    @Index
);
SELECT MAX([id]) FROM [document_directories]