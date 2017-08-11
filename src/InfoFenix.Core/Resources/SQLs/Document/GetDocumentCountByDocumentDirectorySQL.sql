SELECT
    COUNT([document_id])
FROM [documents]
WHERE
    [document_directory_id] = @document_directory_id
AND (@index IS NULL OR ([index] = @index))