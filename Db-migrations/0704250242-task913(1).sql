ALTER PROCEDURE [dbo].[GetDisasterLocationsByCountys]
    @CountyId UNIQUEIDENTIFIER,
    @PageNumber INT = 1,
    @PageSize INT = 10
AS
BEGIN
    SET NOCOUNT ON;

    -- Calculate the offset
    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;

    -- Get paginated results
    WITH PaginatedData AS (
        SELECT 
            lp.Name AS LocationName,
            COALESCE(a.Address, '') + ' ' + COALESCE(a.Address2, '') + ', ' + 
            COALESCE(a.City, '') + ', ' + COALESCE(a.State, '') + ' ' + COALESCE(a.Zip, '') + ', ' +
            c.Name AS FullAddress,  -- Added county name to the address
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
        LEFT JOIN County c ON a.CountyId = c.CountyId  -- Join to get county name
        WHERE lp.IsActive = 1
        AND a.CountyId = @CountyId  -- Filter by CountyId from Address table
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