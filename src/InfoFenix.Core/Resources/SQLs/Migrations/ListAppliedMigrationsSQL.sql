SELECT
    [version],
    [date]
FROM [migrations]
WHERE
    (@version IS NULL OR ([version] = @version));