ALTER PROCEDURE GetGeoJsonByDisaster 
    @EventId UNIQUEIDENTIFIER,
    @LocationTypeId UNIQUEIDENTIFIER = NULL,  -- Changed to GUID
    @ParentTypeId UNIQUEIDENTIFIER = NULL,    -- Changed to GUID
    @StatusId UNIQUEIDENTIFIER = NULL         -- Changed to GUID
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 500  
        'Feature' AS [type],  
        'Point' AS [geometry.type],  
        JSON_QUERY  
        (  
            FORMATMESSAGE('[%s,%s]',  
            FORMAT(CAST(A.Longitude AS DECIMAL(18,15)), N'0.##################################################'),  
            FORMAT(CAST(A.Latitude AS DECIMAL(18,15)), N'0.##################################################'))  
        ) AS [geometry.coordinates],  
        E.[Name] AS [properties.EventName],  
        LP.Name AS [properties.LocationName],  
        LPT.[Name] AS [properties.LocationType],  
        LP.Description AS [properties.Description],  
        A.FormattedAddress AS [properties.Address],  
        A.IsActive AS [properties.IsActive],  
        LP.AllowsPets AS [properties.AllowsPets],  
        LP.Capacity AS [properties.Capacity],  
        LP.ProvidesMedicalHelp AS [properties.ProvidesMedicalHelp],  
        LPT.MapsMarkerPath + LPT.MapsMarkerType AS [properties.icon],  
        LP.LocationProfileId AS [properties.locationProfileId],  
        LP.DonationURL AS [properties.donationURL],  
        LP.SeekingVolunteers AS [properties.SeekingVolunteers],  
        LS.[Name] AS [properties.Status]  
    FROM [Address] A  
    JOIN LocationProfile LP ON A.AddressId = LP.AddressId  
    JOIN LocationProfileEvent LPE ON LP.LocationProfileId = LPE.LocationProfileId  
    JOIN LocationParentType LPT ON LPT.LocationParentTypeId = LP.LocationParentTypeId  
    JOIN LocationLocationType LLT ON A.AddressId = LLT.AddressId  
    JOIN LocationType LT ON LT.LocationTypeId = LLT.LocationTypeId  
    JOIN [Event] E ON E.EventId = LPE.EventId  
    JOIN LocationLocationStatus LLS ON A.AddressId = LLS.AddressId  
    JOIN LocationStatus LS ON LLS.LocationStatusId = LS.LocationStatusId  
    WHERE A.Longitude IS NOT NULL  
        AND E.EventId = @EventId  
        AND LPE.EventId = @EventId  
        AND A.IsActive = 1  
        AND LP.IsActive = 1  
        AND LP.IsOnMap = 1  
        AND (@LocationTypeId IS NULL OR LT.LocationTypeId = @LocationTypeId)  -- Changed to GUID
        AND (@ParentTypeId IS NULL OR LPT.LocationParentTypeId = @ParentTypeId)  -- Changed to GUID
        AND (@StatusId IS NULL OR LS.LocationStatusId = @StatusId);  -- Changed to GUID
END;
