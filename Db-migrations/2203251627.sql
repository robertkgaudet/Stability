CREATE PROCEDURE GetPeopleList    
    @OrganizationId UNIQUEIDENTIFIER,    
    @StartDate DATE = NULL,    
    @EndDate DATE = NULL,    
    @SelectedSkills VARCHAR(MAX) = NULL, -- A comma-separated string of Skill IDs    
    @SelectedResources VARCHAR(MAX) = NULL, -- A comma-separated string of Resource IDs    
    @NameSearchTerm VARCHAR(255) = NULL,    
    @EmailConnected BIT = 0,    
    @IsVetted BIT = 0,    
    @OptedSMS BIT = 0    
AS    
BEGIN    
    SET NOCOUNT ON;    
    
    -- Declare temp table for skills and resources filtering    
    DECLARE @SkillIds TABLE (SkillId UNIQUEIDENTIFIER);    
    DECLARE @ResourceIds TABLE (ResourceId UNIQUEIDENTIFIER);    
    DECLARE @UserIds TABLE (UserId UNIQUEIDENTIFIER);    
    
    -- Parse comma-separated skill IDs into @SkillIds table variable    
    IF (@SelectedSkills != '')    
    BEGIN    
        INSERT INTO @SkillIds (SkillId)    
        SELECT value FROM STRING_SPLIT(@SelectedSkills, ',');    
    END    
    
    -- Parse comma-separated resource IDs into @ResourceIds table variable    
    IF (@SelectedResources != '')    
    BEGIN    
        INSERT INTO @ResourceIds (ResourceId)    
        SELECT value FROM STRING_SPLIT(@SelectedResources, ',');    
    END    
    
    -- Fetch the UserIds matching selected skills or resources    
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
        SET @HasUserIds = 1;    
    
    -- Main query to select people list with various filtering conditions    
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
        p.ReceiveDeploymentSMS,    
        uad.DateAvailable    
    FROM UserOrganization uo    
    JOIN Profile p ON uo.UserId = p.UserId    
    JOIN aspnet_Membership net ON p.UserId = net.UserId    
    JOIN aspnet_Users u ON p.UserId = u.UserId    
    LEFT JOIN UserAvailableDate uad ON p.UserId = uad.UserId    
    WHERE uo.OrganizationId = @OrganizationId    
      AND net.IsLockedOut = 0    
      AND (@StartDate = '' OR uad.DateAvailable >= @StartDate)    
      AND (@EndDate = ''OR uad.DateAvailable <= @EndDate)    
      AND (@IsVetted = 0 OR p.VettingActive = 1)    
      AND (@EmailConnected = 0 OR net.LoweredEmail  != '' AND net.LoweredEmail != '')    
      AND (@OptedSMS = 0 OR p.ReceiveDeploymentSMS = 1)    
      AND (@NameSearchTerm = '' OR    
            (LOWER(p.Firstname) LIKE '%' + LOWER(@NameSearchTerm) + '%'    
             OR LOWER(p.Lastname) LIKE '%' + LOWER(@NameSearchTerm) + '%'  
     OR LOWER(LTRIM(RTRIM(p.Firstname))+' '+ LTRIM(RTRIM(p.Lastname))) LIKE '%' + LOWER(@NameSearchTerm) + '%'))   
      AND (    
            @HasUserIds = 0  -- If no skills/resources were selected, return all users    
            OR EXISTS (SELECT 1 FROM @UserIds uids WHERE uids.UserId = p.UserId)    
          )    
    ORDER BY net.LastLoginDate DESC;    
END    