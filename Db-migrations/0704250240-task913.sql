-- 1. First, add the CountyId column if not already added
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('[dbo].[Address]') AND name = 'CountyId')
BEGIN
    ALTER TABLE [dbo].[Address]
    ADD [CountyId] UNIQUEIDENTIFIER NULL;
END

-- 3. Update with both County (cleaned) and State matching
UPDATE a
SET a.CountyId = c.CountyId
FROM [dbo].[Address] a
JOIN [dbo].[County] c ON 
    REPLACE(REPLACE(a.County, ' County', ''), ' Parish', '') = c.Name
    AND a.State = c.State -- Added state matching
WHERE a.County IS NOT NULL;


-- Use this only in SSMS or SQLCMD, not SSDT (Visual Studio DB Projects)

-- Separate any logic into batches using GO

GO
CREATE OR ALTER PROCEDURE [dbo].[GetDisasterLocationsByCountys]
    @CountyId UNIQUEIDENTIFIER,
    @PageNumber INT = 1,
    @PageSize INT = 10
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;

    WITH PaginatedData AS (
        SELECT 
            lp.LocationProfileId,
            lp.Name AS LocationName,
            COALESCE(a.Address, '') + ' ' + COALESCE(a.Address2, '') + ', ' + 
            COALESCE(a.City, '') + ', ' + COALESCE(a.State, '') + ' ' + COALESCE(a.Zip, '') + ', ' +
            c.Name AS FullAddress,
            a.GooglePlaceId AS GooglePlacesID,
            COALESCE(lp.Description, 'No Description') AS Description,
            CASE 
                WHEN lp.IsOnMap = 1 AND a.Latitude IS NOT NULL AND a.Longitude IS NOT NULL 
                THEN CAST(a.Latitude AS NVARCHAR(20)) + ', ' + CAST(a.Longitude AS NVARCHAR(20))
                ELSE 'Not on Map'
            END AS Coordinates,
            COALESCE(lp.PointOfContactName, 'Not Provided') AS PointOfContact,
            COALESCE(lp.PointOfContactPhoneNumber, 'N/A') AS PhoneNumber,
            COALESCE(lp.PointOfContactEmail, 'N/A') AS Email,
            ROW_NUMBER() OVER (ORDER BY lp.Name) AS RowNum,
            COUNT(*) OVER () AS TotalCount
        FROM LocationProfile lp
        LEFT JOIN Address a ON lp.AddressId = a.AddressId
        LEFT JOIN County c ON a.CountyId = c.CountyId
        WHERE lp.IsActive = 1
          AND a.CountyId = @CountyId
          AND a.GooglePlaceId IS NOT NULL
    )
    SELECT 
        LocationProfileId,  
        LocationName,
        FullAddress,
        GooglePlacesID,
        Description,
        Coordinates,
        PointOfContact,
        PhoneNumber,
        Email,
        TotalCount
    FROM PaginatedData
    WHERE RowNum > @Offset AND RowNum <= @Offset + @PageSize
    ORDER BY RowNum;
END
GO
