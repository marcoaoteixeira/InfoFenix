SELECT
    [version],
    [date]
FROM [migrations]
WHERE
    [version] = @version;