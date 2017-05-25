SELECT
    [id],
    [label],
    [path],
    [code],
    [watch],
    [index]
FROM [document_directories]
WHERE
    (@Label IS NULL OR ([label] LIKE '%' + @Label + '%'))
AND (@Path IS NULL OR ([path] LIKE '%' + @Path + '%'))
AND (@Code IS NULL OR ([code] LIKE '%' + @Code + '%'))
AND (@Watch IS NULL OR ([watch] = @Watch))
AND (@Index IS NULL OR ([index] = @Index));