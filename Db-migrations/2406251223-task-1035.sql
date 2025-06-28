-- ================================
-- Table: OrganizationWaiver
-- ================================
-- Single waiver per organization
CREATE TABLE OrganizationWaiver (
    OrganizationId UNIQUEIDENTIFIER PRIMARY KEY,
    WaiverText VARCHAR(MAX) NULL,
    IsRequired BIT NOT NULL DEFAULT 0,
    UpdatedOn DATETIME NOT NULL DEFAULT GETDATE(),
    UpdatedBy UNIQUEIDENTIFIER NOT NULL,

    FOREIGN KEY (OrganizationId) REFERENCES Organization(OrganizationId),
    FOREIGN KEY (UpdatedBy) REFERENCES aspnet_Users(UserId)
);

-- ================================
-- Table: WaiverSignature
-- ================================
-- Signature records
CREATE TABLE WaiverSignature (
    SignatureId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    OrganizationId UNIQUEIDENTIFIER NOT NULL,
    UserId UNIQUEIDENTIFIER NOT NULL,
    SignatureName VARCHAR(500)NOT  NULL,
    SignedOn DATETIME NOT NULL DEFAULT GETDATE(),

    FOREIGN KEY (OrganizationId) REFERENCES Organization(OrganizationId),
    FOREIGN KEY (UserId) REFERENCES aspnet_Users(UserId),

    -- One signature per user per organization
    CONSTRAINT UC_UserOrgSignature UNIQUE (UserId, OrganizationId)
);

--update sp--
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[GetPeopleList]  --  '79305f85-3816-46a8-911f-0d7e3e227c32','','','','','','',0,0,100,false,false,false,false,false,1,50          
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
    @stabilityVerified BIT = 0,     
    @teamAdministrator BIT=0,  
    @PageNumber INT = 1,        
    @PageSize INT = 10        
AS              
BEGIN              
    SET NOCOUNT ON;              
        
    DECLARE @SkillIds TABLE (SkillId UNIQUEIDENTIFIER);              
    DECLARE @ResourceIds TABLE (ResourceId UNIQUEIDENTIFIER);              
    DECLARE @UserIds TABLE (UserId UNIQUEIDENTIFIER);              
        
    IF (@SelectedSkills != '')              
    BEGIN              
        INSERT INTO @SkillIds (SkillId)              
        SELECT value FROM STRING_SPLIT(@SelectedSkills, ',');              
    END              
        
    IF (@SelectedResources != '')              
    BEGIN              
        INSERT INTO @ResourceIds (ResourceId)              
        SELECT value FROM STRING_SPLIT(@SelectedResources, ',');              
    END              
        
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
        
    IF (@position != '')             
    BEGIN                
        INSERT INTO @UserIds (UserId)          
        SELECT DISTINCT uoep.UserId            
        FROM Position po          
        LEFT JOIN OrganizationEventPosition oep ON po.PositionId = oep.PositionId          
        LEFT JOIN UserOrganizationEventPosition uoep ON oep.OrganizationEventPositionId = uoep.OrganizationEventPositionId          
        WHERE po.PositionId = @position;                
    END            
        
    INSERT INTO @UserIds (UserId)              
    SELECT DISTINCT us.UserId              
    FROM UserSkill us              
    WHERE us.SkillId IN (SELECT SkillId FROM @SkillIds)              
    UNION              
    SELECT DISTINCT ur.UserId              
    FROM UserResource ur              
    WHERE ur.ResourceId IN (SELECT ResourceId FROM @ResourceIds);              
        
    DECLARE @HasUserIds BIT = 0;              
    IF EXISTS (SELECT 1 FROM @UserIds)            
        OR @SelectedResources != ''           
        OR @SelectedSkills != ''           
        OR @position != ''           
        OR @lat != 0           
        SET @HasUserIds = 1;              
        
    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;        
  ;WITH FilteredUserIds AS (        
    SELECT DISTINCT p.UserId, net.LastLoginDate        
    FROM UserOrganization uo              
    JOIN Profile p ON uo.UserId = p.UserId              
    JOIN aspnet_Membership net ON p.UserId = net.UserId              
    JOIN aspnet_Users u ON p.UserId = u.UserId              
    LEFT JOIN UserAvailableDate uad ON p.UserId = uad.UserId              
    WHERE uo.OrganizationId = @OrganizationId  AND uo.Status=1           
        AND net.IsLockedOut = 0            
          AND (@StartDate = '' OR uad.DateAvailable >= @StartDate)              
          AND (@EndDate = '' OR uad.DateAvailable <= @EndDate)              
          AND (@IsVetted = 0 OR p.VettingActive = 1)              
          AND (@EmailConnected = 0 OR net.LoweredEmail != '')              
          AND (@OptedSMS = 0 OR p.ReceiveSMSNotifications = 1)              
          AND (@teamVerified = 0 OR uo.ShowTeamLogo = 1)          
          AND (@stabilityVerified = 0 OR p.IsDisasterReadyCertified = 1)     
    AND (@teamAdministrator = 0 OR uo.IsTeamAdministrator = 1)  
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
) ,        
PagedUserIds AS (        
    SELECT UserId, LastLoginDate,        
           COUNT(*) OVER() AS TotalCount        
    FROM FilteredUserIds        
    ORDER BY LastLoginDate DESC        
    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY        
)      
        
    SELECT              
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
        p.ReceiveSMSNotifications,        
        pu.TotalCount  ,
		our.RankPosition,
		ws.SignatureName
    FROM PagedUserIds pu        
    JOIN Profile p ON pu.UserId = p.UserId        
    JOIN UserOrganization uo ON p.UserId = uo.UserId              
    JOIN aspnet_Membership net ON p.UserId = net.UserId              
    JOIN aspnet_Users u ON p.UserId = u.UserId        
    LEFT JOIN OrganizationUserRanking our 
    ON our.UserId = uo.UserId
	LEFT JOIN WaiverSignature ws ON ws.UserId = p.UserId AND ws.OrganizationId = @OrganizationId
    WHERE uo.OrganizationId = @OrganizationId AND Status=1     
    ORDER BY net.LastLoginDate DESC;        
END 