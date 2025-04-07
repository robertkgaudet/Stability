
  ALTER TABLE [dbo].[Address]
ADD [CountyId] UNIQUEIDENTIFIER NULL;

-- 4. Verify all CountyIds are cleared
SELECT 
    COUNT(*) AS TotalAddresses,
    SUM(CASE WHEN CountyId IS NULL THEN 1 ELSE 0 END) AS NullCountyIds,
    SUM(CASE WHEN CountyId IS NOT NULL THEN 1 ELSE 0 END) AS RemainingCountyIds
FROM[dbo].[Address];

-- 5. Now you can run your update with clean matching logic
UPDATE a
SET a.CountyId = c.CountyId
FROM [dbo].[Address] a
JOIN [dbo].[County] c ON 
    REPLACE(REPLACE(a.County, ' County', ''), ' Parish', '') = c.Name
WHERE a.County IS NOT NULL;

-- 6. Verify the new updates
SELECT 
    COUNT(*) AS TotalAddresses,
    SUM(CASE WHEN CountyId IS NOT NULL THEN 1 ELSE 0 END) AS UpdatedRecords,
    SUM(CASE WHEN CountyId IS NULL AND County IS NOT NULL THEN 1 ELSE 0 END) AS RemainingNulls
FROM [dbo].[Address];