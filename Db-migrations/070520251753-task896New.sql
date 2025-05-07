ALTER PROCEDURE [dbo].[MapStabilityLocations]  
    @EventId UNIQUEIDENTIFIER,  
    @Status UNIQUEIDENTIFIER = NULL,  -- Changed from NVARCHAR(255) to UNIQUEIDENTIFIER  
    @LocationTypeId UNIQUEIDENTIFIER = NULL
AS  
BEGIN  
    SET NOCOUNT ON;  

    DECLARE @result NVARCHAR(MAX);  

    SET @result = (  
        SELECT TOP 500  
            'Feature' AS [type],  
            'Point' AS [geometry.type],  
            JSON_QUERY(  
                FORMATMESSAGE('[%s,%s]',  
                    FORMAT(CAST(A.Longitude AS DECIMAL(18,15)), N'0.##################################################'),  
                    FORMAT(CAST(A.Latitude AS DECIMAL(18,15)), N'0.##################################################'))  
            ) AS [geometry.coordinates],  
            A.FormattedAddress AS [properties.Address],  
            A.IsActive AS [properties.IsActive],  
            OE.CampaignName AS [properties.CampaignName],  
            OE.LogoFileName AS [properties.LogoFileName],  
            OE.URLFriendlyCampaignName AS [properties.URLFriendlyCampaignName],  
            OE.MissionPurpose AS [properties.MissionPurpose],  
            OE.PointOfContactName AS [properties.PointOfContactName],  
            OE.PhoneNumber AS [properties.PhoneNumber],  
            O.[Name] AS [properties.OrganizationName],  
            O.[OrganizationId] AS [properties.OrganizationId],  
            O.[URLFriendlyName] AS [properties.URLFriendlyOrganizationName],  
            LS.[LocationStatusId] AS [properties.Status],  -- Now returning GUID instead of Name  
            LT.[LocationTypeId] AS [properties.LocationType],  -- GUID instead of Name              
            CASE  
                WHEN O.LogoSquare IS NULL THEN '/V1/Images/Logo-Placeholder.png'  
                ELSE '/Impactoid/Images/Logos/' + O.LogoSquare  
            END AS [properties.icon]  
        FROM [Address] A  
        JOIN OrganizationEvent OE ON A.AddressId = OE.AddressId  
        JOIN Organization O ON O.OrganizationId = OE.OrganizationId  
        JOIN LocationLocationStatus LLS ON A.AddressId = LLS.AddressId  
        JOIN LocationStatus LS ON LLS.LocationStatusId = LS.LocationStatusId  
        -- Added Join to fetch LocationType from LocationLocationType table  
        JOIN LocationLocationType LLT ON A.AddressId = LLT.AddressId  
        JOIN LocationType LT ON LLT.LocationTypeId = LT.LocationTypeId         
        WHERE A.Longitude IS NOT NULL  
            AND OE.EventId = @EventId  
            AND A.IsActive = 1  
            AND OE.IsActive = 1  
            AND (@Status IS NULL OR LS.LocationStatusId = @Status)  -- Filter using GUID  
            AND (@LocationTypeId IS NULL OR LLT.LocationTypeId = @LocationTypeId)  -- Filter using GUID              
        FOR JSON PATH  
    );  

    DECLARE @outer NVARCHAR(MAX) = (  
        SELECT 'FeatureCollection' AS [type],  
        JSON_QUERY(@result) AS 'features'  
        FOR JSON PATH, WITHOUT_ARRAY_WRAPPER  
    );  

    SELECT @outer;  
END;  
