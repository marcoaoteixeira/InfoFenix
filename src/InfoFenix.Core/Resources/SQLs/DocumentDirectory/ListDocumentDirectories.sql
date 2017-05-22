SELECT
    [document_directory_id],
    [label],
    [directory_path],
    [code],
    [watch],
    [index]
FROM [document_directories]
WHERE
    (@Label IS NULL OR ([label] LIKE '%' + @Label + '%'))
AND (@DirectoryPath IS NULL OR ([directory_path] LIKE '%' + @DirectoryPath + '%'))
AND (@Code IS NULL OR ([code] LIKE '%' + @Code + '%'))
AND (@Watch IS NULL OR ([watch] = @Watch))
AND (@Index IS NULL OR ([index] = @Index));