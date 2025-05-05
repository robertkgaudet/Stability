ALTER PROCEDURE [dbo].[GetGeoJsonByDisaster]  
    @EventId UNIQUEIDENTIFIER,  
    @LocationTypeId UNIQUEIDENTIFIER = NULL,  
    @ParentTypeId UNIQUEIDENTIFIER = NULL,  
    @StatusId UNIQUEIDENTIFIER = NULL  
AS  
BEGIN  
    SET NOCOUNT ON;  
  
    DECLARE @result NVARCHAR(MAX);  
  
    SET @result = (  
        SELECT TOP 500    
            'Feature' AS [type],    
            'Point' AS [geometry.type],    
            JSON_QUERY(FORMATMESSAGE('[%s,%s]',  
                FORMAT(CAST(A.Longitude AS DECIMAL(18,15)), N'0.##################################################'),    
                FORMAT(CAST(A.Latitude AS DECIMAL(18,15)), N'0.##################################################'))  
            ) AS [geometry.coordinates],    
            E.[Name] AS [properties.EventName],    
            LP.Name AS [properties.LocationName],    
            LPT.[Name] AS [properties.LocationType],
			LPT.[LocationParentTypeId] AS [properties.LocationParentTypeId],
			LT.[Name] AS [properties.LocationTypeName],
			LT.[LocationTypeId] AS [properties.LocationTypeId],
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
            LS.[Name] AS [properties.Status],
			LS.[LocationStatusId] AS [properties.LocationStatusId]
        FROM [Address] A    
        JOIN LocationProfile LP ON A.AddressId = LP.AddressId    
        JOIN LocationProfileEvent LPE ON LP.LocationProfileId = LPE.LocationProfileId    
        JOIN LocationParentType LPT ON LPT.LocationParentTypeId = LP.LocationParentTypeId    
        JOIN LocationLocationType LLT ON LP.AddressId = LLT.AddressId    
        JOIN LocationType LT ON LT.LocationTypeId = LLT.LocationTypeId    
        LEFT JOIN LocationLocationStatus LLS ON A.AddressId = LLS.AddressId  -- Corrected join condition  
        LEFT JOIN LocationStatus LS ON LLS.LocationStatusId = LS.LocationStatusId  
        JOIN [Event] E ON E.EventId = LPE.EventId    
        WHERE A.Longitude IS NOT NULL    
            AND E.EventId = @EventId    
            AND LPE.EventId = @EventId    
            AND A.IsActive = 1    
            AND LP.IsActive = 1    
            AND LP.IsOnMap = 1    
            AND (@LocationTypeId IS NULL OR LT.LocationTypeId = @LocationTypeId)  
            AND (@ParentTypeId IS NULL OR LPT.LocationParentTypeId = @ParentTypeId)  
            AND (@StatusId IS NULL OR LS.LocationStatusId = @StatusId)  
        FOR JSON PATH  
    );  
  
    DECLARE @outer NVARCHAR(MAX) = (  
        SELECT 'FeatureCollection' AS [type],  
        JSON_QUERY(@result) AS 'features'  
        FOR JSON PATH, WITHOUT_ARRAY_WRAPPER  
    );  
  
    SELECT @outer;  
END