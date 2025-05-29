/*please add this*/
 ALTER TABLE[dbo].[UserOrganization]
ADD Status INT NOT NULL DEFAULT 0


EXEC sp_rename 'UserOrganization.TeamJoinStatus', 'status', 'COLUMN';

UPDATE uo
SET uo.IsOwner = 1
FROM UserOrganization uo
INNER JOIN Organization o
    ON uo.OrganizationId = o.OrganizationId
    AND uo.UserId = o.OwnerId;



    ALTER TABLE [dbo].[Notification]
ADD OrganizationId UNIQUEIDENTIFIER NULL;



  INSERT INTO [DB_8013_development].[dbo].[FeatureType]
(
    [FeatureName],
    [RedirectURL],
    [IncludeInStream],
    [Counter],
    [DisplayText]
)
VALUES
(  
    'TeamRequest',
    '/V1/NonProfit/ReceivedRequests.aspx?organizationId=',
    0,              
    0,             
    'TeamRequest'
);