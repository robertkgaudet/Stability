
Create PROCEDURE [dbo].[SearchDeploymentTeam]
    @Location NVARCHAR(100) = NULL,
    @Team NVARCHAR(100) = NULL,
    @DeploymentDateFrom NVARCHAR(10) = NULL,  -- Note: changed type to NVARCHAR(10)
    @DeploymentDateTo NVARCHAR(10) = NULL,
    @Position NVARCHAR(200) = NULL,
    @PageNumber INT = 1,
    @PageSize INT = 10
AS
BEGIN
    SET NOCOUNT ON;

    SET @Location = NULLIF(LTRIM(RTRIM(@Location)), '')
    SET @Team = NULLIF(LTRIM(RTRIM(@Team)), '')
    SET @Position = NULLIF(LTRIM(RTRIM(@Position)), '')

    -- Convert date strings to DATE, NULL if invalid or empty
    DECLARE @DateFrom DATE = NULL;
    DECLARE @DateTo DATE = NULL;

    IF ISDATE(@DeploymentDateFrom) = 1
        SET @DateFrom = CONVERT(DATE, @DeploymentDateFrom);

    IF ISDATE(@DeploymentDateTo) = 1
        SET @DateTo = CONVERT(DATE, @DeploymentDateTo);

    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;

    SELECT 
        COUNT(*) OVER() AS TotalCount,
        oe.OrganizationEventId,
        oe.CampaignName,
		p.Name,
		o.Name,
        oep.DeploymentDate,
        oe.URLFriendlyCampaignName,
        county.Name AS County,
        county.State AS State,
        city.City AS City,
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
    INNER JOIN Organization o ON oe.OrganizationId = o.OrganizationId
    INNER JOIN Position p ON oep.PositionId = p.PositionId
    INNER JOIN County county ON oe.StagingCountyId = county.CountyId
    INNER JOIN City city ON county.Code = city.Code
    WHERE
        oe.IsActive = 1
        AND (
            @Location IS NULL OR
            city.City LIKE '%' + @Location + '%' OR
            county.State LIKE '%' + @Location + '%' OR
            county.Name LIKE '%' + @Location + '%'
        )
        AND (
            @Team IS NULL OR o.Name = @Team
        )
        AND (
            @DateFrom IS NULL OR oep.DeploymentDate >= @DateFrom
        )
        AND (
            @DateTo IS NULL OR oep.DeploymentDate <= @DateTo
        )
        AND (
            @Position IS NULL OR p.Name LIKE '%' + @Position + '%'
        )
    ORDER BY oep.DeploymentDate DESC
    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
END
