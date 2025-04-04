-- 1. First, add the CountyId column if not already added
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('[dbo].[Address]') AND name = 'CountyId')
BEGIN
    ALTER TABLE [dbo].[Address]
    ADD [CountyId] UNIQUEIDENTIFIER NULL;
END

-- 2. Verify initial state
SELECT 
    COUNT(*) AS TotalAddresses,
    SUM(CASE WHEN CountyId IS NULL THEN 1 ELSE 0 END) AS NullCountyIds,
    SUM(CASE WHEN CountyId IS NOT NULL THEN 1 ELSE 0 END) AS RemainingCountyIds
FROM [dbo].[Address];

-- 3. Update with both County (cleaned) and State matching
UPDATE a
SET a.CountyId = c.CountyId
FROM [dbo].[Address] a
JOIN [dbo].[County] c ON 
    REPLACE(REPLACE(a.County, ' County', ''), ' Parish', '') = c.Name
    AND a.State = c.State -- Added state matching
WHERE a.County IS NOT NULL;

-- 4. Verification query with state comparison
SELECT TOP (1000)
    a.County AS AddressCounty,
    a.CountyId AS AddressCountyId,
    c.Name AS CountyName,
    c.CountyId AS CountyTableCountyId,
    c.State AS CountyState,
    a.State AS AddressState,
    CASE 
        WHEN a.State = c.State THEN 'State Match' 
        ELSE 'State Mismatch' 
    END AS StateComparison
FROM [dbo].[Address] a
LEFT JOIN [dbo].[County] c ON a.CountyId = c.CountyId
ORDER BY StateComparison;