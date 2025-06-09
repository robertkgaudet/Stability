EXEC sp_rename 'dbo.SearchFillter', 'NavSearchFilter';



SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[NavSearchFilter]
    @searchType NVARCHAR(50),
    @searchTerm NVARCHAR(100),
	 @PageNumber INT = NULL,
    @PageSize INT = NULL
AS
BEGIN
    -- Teams search
    IF @searchType = 'Teams'
    BEGIN
        SELECT *
        FROM Organization
        WHERE 
            (@searchTerm IS NULL OR LTRIM(RTRIM(@searchTerm)) = '' OR Name LIKE '%' + @searchTerm + '%')
            AND IsActive = 1
    END
 
    -- Deployments search
    ELSE IF @searchType = 'Deployments'
    BEGIN
        SELECT 
            oe.CampaignName,
            oe.BeginDate,
            oe.EndDate,
            o.Name,
            oe.MissionPurpose,
            o.OrganizationId,
            oe.OrganizationEventId,
            o.Logo,
            oe.CreatedOn
        FROM OrganizationEvent oe
        JOIN Organization o ON oe.OrganizationId = o.OrganizationId
        WHERE 
            @searchTerm IS NULL OR LTRIM(RTRIM(@searchTerm)) = '' OR 
            o.Name LIKE '%' + @searchTerm + '%' OR 
            oe.CampaignName LIKE '%' + @searchTerm + '%'
			 ORDER BY  oe.CreatedOn DESC;
    END
	-- Deployments search End
    -- Skills search
    ELSE IF @searchType = 'Skills'
    BEGIN
        SELECT
		s.SkillId, s.Name as SkillName,
		p.UserId,
            (
                SELECT TOP 1 ph.FilenameCropped
                FROM Photo ph
                INNER JOIN ProfilePhoto pp ON ph.PhotoId = pp.PhotoId
                WHERE pp.UserId = p.UserId
                ORDER BY ph.CreatedOn DESC
            ) AS ProfileImage,
            p.Firstname + ' ' + p.Lastname AS FullName,
            ISNULL(p.PassedVetting, 0) AS PassedVetting,
            p.Description AS ProfileDescription,
            p.Title AS ProfileTitle,
            p.City + ' ' + p.State AS CityState,
            (
                SELECT TOP 1 o.Name
                FROM UserOrganization uo
                INNER JOIN Organization o ON uo.OrganizationId = o.OrganizationId
                WHERE uo.UserId = p.UserId AND uo.Status = 1
            ) AS TeamName
        FROM Skill s
 
INNER JOIN UserSkill us ON s.SkillId = us.SkillId
INNER JOIN Profile p ON p.UserId = us.UserId
        WHERE @searchTerm IS NULL OR LTRIM(RTRIM(@searchTerm)) = '' OR Name LIKE '%' + @searchTerm + '%'
    END
	 -- Skills search End
    -- Resources search
    ELSE IF @searchType = 'Resources'
    BEGIN
         SELECT
		R.ResourceId, R.Name as ResourceName,
		p.UserId,
            (
                SELECT TOP 1 ph.FilenameCropped
                FROM Photo ph
                INNER JOIN ProfilePhoto pp ON ph.PhotoId = pp.PhotoId
                WHERE pp.UserId = p.UserId
                ORDER BY ph.CreatedOn DESC
            ) AS ProfileImage,
            p.Firstname + ' ' + p.Lastname AS FullName,
            ISNULL(p.PassedVetting, 0) AS PassedVetting,
            p.Description AS ProfileDescription,
            p.Title AS ProfileTitle,
            p.City + ' ' + p.State AS CityState,
            (
                SELECT TOP 1 o.Name
                FROM UserOrganization uo
                INNER JOIN Organization o ON uo.OrganizationId = o.OrganizationId
                WHERE uo.UserId = p.UserId AND uo.Status = 1
            ) AS TeamName
        FROM Resource R
 
INNER JOIN UserResource ur ON R.ResourceId = ur.ResourceId
INNER JOIN Profile p ON p.UserId = ur.UserId
        WHERE 
            @searchTerm IS NULL OR LTRIM(RTRIM(@searchTerm)) = '' OR 
           R.Name LIKE '%' + @searchTerm + '%' OR 
           R.Type LIKE '%' + @searchTerm + '%'
        ORDER BY Name, Type;
    END
	-- Resources search End 
    -- Portals search
    ELSE IF @searchType = 'Portals'
    BEGIN
        SELECT 
            ' - ' + Name AS name,
            EventId
        FROM Event
        WHERE 
            (@searchTerm IS NULL OR LTRIM(RTRIM(@searchTerm)) = '' OR Name LIKE '%' + @searchTerm + '%')
            AND IsDisaster = 1
            AND IsActive = 1
        ORDER BY BeginDate DESC;
    END
	  -- Portals search End
	 --Volunteer Opportunities
	 ELSE IF @searchType = 'VolunteerOpportunities'
    BEGIN
DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;
  SELECT 
  COUNT(*) OVER() AS TotalCount,  
        oe.OrganizationEventId,
        oe.CampaignName,
        oep.DeploymentDate,
        oe.URLFriendlyCampaignName,
        county.Name AS County,
        county.State As State,
        city.city AS City,
		  (
            SELECT MIN(innerOep.DeploymentDate)
            FROM OrganizationEventPosition innerOep
            WHERE innerOep.OrganizationEventId = oe.OrganizationEventId
        ) AS EarliestDeploymentDate,
        (
            SELECT MAX(innerOep.DeploymentDate)
            FROM OrganizationEventPosition innerOep
            WHERE innerOep.OrganizationEventId = oe.OrganizationEventId
        ) AS LatestDeploymentDate,
        (
            SELECT COUNT(DISTINCT innerOep.PositionId)
            FROM OrganizationEventPosition innerOep
            WHERE innerOep.OrganizationEventId = oe.OrganizationEventId
        ) AS PositionCount,
        (
            SELECT TOP 1 innerOep.DeploymentDate
            FROM OrganizationEventPosition innerOep
            WHERE innerOep.OrganizationEventId = oe.OrganizationEventId
            ORDER BY innerOep.DeploymentDate ASC
        ) AS RowNum
 
 
  
 
    FROM OrganizationEventPosition oep
    INNER JOIN OrganizationEvent oe ON oep.OrganizationEventId = oe.OrganizationEventId
    INNER JOIN Position p ON oep.PositionId = p.PositionId
    INNER JOIN County county ON oe.StagingCountyId = county.CountyId
    INNER JOIN City city ON county.Code = city.Code
    WHERE 
        oe.IsActive = 1
		OR @searchTerm IS NULL OR LTRIM(RTRIM(@searchTerm)) = '' OR 
		oe.CampaignName LIKE '%' + @searchTerm + '%' OR
			oe.OrganizationId LIKE '%' + @searchTerm + '%' OR
               oe.URLFriendlyCampaignName LIKE '%' + @searchTerm + '%'
			   ORDER BY oep.DeploymentDate DESC
        OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
	 END
 
      
		 --Volunteer Opportunities End
	 ELSE IF @searchType = 'Posts'
    BEGIN
       SELECT 
    p.CreatedBy,
    p.PostId,
    p.PostTypeId,
    p.URLImage,
    p.URLTitle,
    p.URLDescription,
    p.SharedURL,
    p.CreatedOn,
    p.Message,
    pr.UserId,
    ISNULL(p.EventId, NEWID()) AS EventId, -- Replacing null with a new GUID (mimicking C#'s new Guid())
    pr.Firstname + ' ' + pr.Lastname AS fullname
FROM 
    Post p
INNER JOIN 
    Profile pr ON p.CreatedBy = pr.UserId
INNER JOIN 
    aspnet_Membership us ON p.CreatedBy = us.UserId
WHERE 
    p.IsVisible = 1 And
	p.URLTitle LIKE '%' + @searchTerm + '%'OR
	p.SharedURL LIKE '%' + @searchTerm + '%'OR
	pr.Firstname LIKE '%' + @searchTerm + '%'OR
	pr.Lastname LIKE '%' + @searchTerm + '%'
ORDER BY 
    p.CreatedOn DESC
    END
	--Posts
    ELSE 
	      -- Profile search
    BEGIN
        SELECT 
            p.UserId,
            (
                SELECT TOP 1 ph.FilenameCropped
                FROM Photo ph
                INNER JOIN ProfilePhoto pp ON ph.PhotoId = pp.PhotoId
                WHERE pp.UserId = p.UserId
                ORDER BY ph.CreatedOn DESC
            ) AS ProfileImage,
            p.Firstname + ' ' + p.Lastname AS FullName,
            ISNULL(p.PassedVetting, 0) AS PassedVetting,
            p.Description AS ProfileDescription,
            m.CreateDate,
            p.Title AS ProfileTitle,
            p.City + ' ' + p.State AS CityState,
            (
                SELECT TOP 1 o.Name
                FROM UserOrganization uo
                INNER JOIN Organization o ON uo.OrganizationId = o.OrganizationId
                WHERE uo.UserId = p.UserId AND uo.Status = 1
            ) AS TeamName
        FROM Profile p
        INNER JOIN aspnet_Membership m ON p.UserId = m.UserId
        WHERE 
            m.IsLockedOut = 0 AND 
            m.IsApproved = 1 AND (
                @searchTerm IS NULL OR LTRIM(RTRIM(@searchTerm)) = '' OR
                p.Firstname + ' ' + p.Lastname LIKE '%' + @searchTerm + '%' OR
                p.Description LIKE '%' + @searchTerm + '%' OR
                p.Title LIKE '%' + @searchTerm + '%' OR
                p.City + ' ' + p.State LIKE '%' + @searchTerm + '%' OR
                EXISTS (
                    SELECT 1
                    FROM UserOrganization uo
                    JOIN Organization o ON uo.OrganizationId = o.OrganizationId
                    WHERE uo.UserId = p.UserId AND uo.Status = 1 AND o.Name LIKE '%' + @searchTerm + '%'
                )
            )
        ORDER BY m.CreateDate DESC;
    END
END