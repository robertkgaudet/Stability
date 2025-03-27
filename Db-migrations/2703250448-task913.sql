CREATE PROCEDURE [dbo].[GetDisasterLocationsByCountys]
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
            COALESCE(a.City, '') + ', ' + COALESCE(a.State, '') + ' ' + COALESCE(a.Zip, '') AS FullAddress,
            a.GooglePlaceId AS GooglePlacesID,
            COALESCE(lp.Description, 'No Description') AS Description,
            CASE 
                WHEN lp.IsOnMap = 1 AND a.Latitude IS NOT NULL AND a.Longitude IS NOT NULL 
                THEN CAST(a.Latitude AS NVARCHAR) + ', ' + CAST(a.Longitude AS NVARCHAR)
                ELSE 'Not on Map'
            END AS Coordinates,
            COALESCE(lp.PointOfContactName, 'Not Provided') AS PointOfContact,
            COALESCE(lp.PointOfContactPhoneNumber, 'N/A') AS PhoneNumber,
            COALESCE(lp.PointOfContactEmail, 'N/A') AS Email,
            ROW_NUMBER() OVER (ORDER BY lp.Name) AS RowNum,
            COUNT(*) OVER () AS TotalCount
        FROM LocationProfile lp
        LEFT JOIN Address a ON lp.AddressId = a.AddressId
        INNER JOIN LocationProfileEvent lpe ON lp.LocationProfileId = lpe.LocationProfileId
        INNER JOIN Event e ON lpe.EventId = e.EventId
        INNER JOIN EventCounty ec ON e.EventId = ec.EventId
        WHERE lp.IsActive = 1
        AND ec.CountyId = @CountyId
        AND a.GooglePlaceId IS NOT NULL
    )
    SELECT 
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