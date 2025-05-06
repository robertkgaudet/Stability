SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[GetPeopleList]    
    @OrganizationId UNIQUEIDENTIFIER,    
    @StartDate DATE = NULL,    
    @EndDate DATE = NULL,    
    @SelectedSkills VARCHAR(MAX) = NULL,    
    @SelectedResources VARCHAR(MAX) = NULL,    
    @NameSearchTerm VARCHAR(255) = NULL, 
    @position VARCHAR(100) = NULL,
    @lat FLOAT = 0,    
    @lng FLOAT = 0,    
    @radius FLOAT = 0,  
    @EmailConnected BIT = 0,    
    @IsVetted BIT = 0,    
    @OptedSMS BIT = 0,
    @teamVerified BIT = 0,
    @stabilityVerified BIT = 0
AS    
BEGIN    
    SET NOCOUNT ON;    
    
    -- Declare temp tables    
    DECLARE @SkillIds TABLE (SkillId UNIQUEIDENTIFIER);    
    DECLARE @ResourceIds TABLE (ResourceId UNIQUEIDENTIFIER);    
    DECLARE @UserIds TABLE (UserId UNIQUEIDENTIFIER);    
    
    -- Parse Selected Skills    
    IF (@SelectedSkills != '')    
    BEGIN    
        INSERT INTO @SkillIds (SkillId)    
        SELECT value FROM STRING_SPLIT(@SelectedSkills, ',');    
    END    
    
    -- Parse Selected Resources    
    IF (@SelectedResources != '')    
    BEGIN    
        INSERT INTO @ResourceIds (ResourceId)    
        SELECT value FROM STRING_SPLIT(@SelectedResources, ',');    
    END    
  
    -- Location Filter    
    IF (@lat != 0 AND @lng != 0 AND @radius != 0)      
    BEGIN      
        INSERT INTO @UserIds (UserId)  
        SELECT DISTINCT p.UserId  
        FROM Address a  
        JOIN ProfileAddress pa ON a.AddressId = pa.AddressId  
        JOIN Profile p ON pa.ProfileId = p.ProfileId  
        WHERE   
            (6371  * ACOS(COS(RADIANS(@lat))  * COS(RADIANS(a.Latitude))   
            * COS(RADIANS(a.Longitude) - RADIANS(@lng)) + SIN(RADIANS(@lat))   
            * SIN(RADIANS(a.Latitude)))) <= @radius;      
    END    
    
    -- Position Filter    
    IF (@position != '')   
    BEGIN      
        INSERT INTO @UserIds (UserId)
        SELECT DISTINCT uoep.UserId  
        FROM Position po
        LEFT JOIN OrganizationEventPosition oep ON po.PositionId = oep.PositionId
        LEFT JOIN UserOrganizationEventPosition uoep ON oep.OrganizationEventPositionId = uoep.OrganizationEventPositionId
        WHERE po.PositionId = @position;      
    END  
    
    -- Skills and Resources Filter    
    INSERT INTO @UserIds (UserId)    
    SELECT DISTINCT us.UserId    
    FROM UserSkill us    
    WHERE us.SkillId IN (SELECT SkillId FROM @SkillIds)    
    UNION    
    SELECT DISTINCT ur.UserId    
    FROM UserResource ur    
    WHERE ur.ResourceId IN (SELECT ResourceId FROM @ResourceIds);    
    
    -- Flag to check if any filters were applied    
    DECLARE @HasUserIds BIT = 0;    
    IF EXISTS (SELECT 1 FROM @UserIds)  
        OR @SelectedResources != '' 
        OR @SelectedSkills != '' 
        OR @position != '' 
        OR @lat != 0 
        SET @HasUserIds = 1;    
    
    -- Main SELECT with filters    
    SELECT DISTINCT    
        p.Firstname,    
        p.Lastname,    
        net.CreateDate,    
        p.Description,    
        net.LoweredEmail,    
        p.PhoneNumber,    
        p.UserId,    
        p.DateVettingCompleted,    
        p.DateVettingStarted,    
        p.VettingNotes,    
        p.VettingActive,    
        p.VettingComplete,    
        p.PassedVetting,    
        p.Title,    
        p.ZelloName,    
        u.LastActivityDate,    
        net.LastLoginDate,    
        net.IsApproved,    
		uo.ShowTeamLogo,
		p.IsDisasterReadyCertified,
		p.ReceiveSMSNotifications
    FROM UserOrganization uo    
    JOIN Profile p ON uo.UserId = p.UserId    
    JOIN aspnet_Membership net ON p.UserId = net.UserId    
    JOIN aspnet_Users u ON p.UserId = u.UserId    
    LEFT JOIN UserAvailableDate uad ON p.UserId = uad.UserId    
    WHERE uo.OrganizationId = @OrganizationId    
      AND net.IsLockedOut = 0  
      AND (@StartDate = '' OR uad.DateAvailable >= @StartDate)    
      AND (@EndDate = '' OR uad.DateAvailable <= @EndDate)    
      AND (@IsVetted = 0 OR p.VettingActive = 1)    
      AND (@EmailConnected = 0 OR net.LoweredEmail != '')    
      AND (@OptedSMS = 0 OR p.ReceiveSMSNotifications = 1)    
      AND (@teamVerified = 0 OR uo.ShowTeamLogo = 1)
      AND (@stabilityVerified = 0 OR p.IsDisasterReadyCertified = 1)
      AND (
            @NameSearchTerm = '' OR    
            (LOWER(p.Firstname) LIKE '%' + LOWER(@NameSearchTerm) + '%'    
             OR LOWER(p.Lastname) LIKE '%' + LOWER(@NameSearchTerm) + '%'
             OR LOWER(LTRIM(RTRIM(p.Firstname)) + ' ' + LTRIM(RTRIM(p.Lastname))) LIKE '%' + LOWER(@NameSearchTerm) + '%')
          )
      AND (
            @HasUserIds = 0  
            OR EXISTS (SELECT 1 FROM @UserIds uids WHERE uids.UserId = p.UserId)
          )       
    ORDER BY net.LastLoginDate DESC;    
END
