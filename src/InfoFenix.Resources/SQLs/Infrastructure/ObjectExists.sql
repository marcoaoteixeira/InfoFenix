SELECT CASE
    WHEN EXISTS(SELECT [name] FROM [sqlite_master] WHERE [type] = @type AND [name] = @name) THEN 1
    ELSE 0
END AS [exists];