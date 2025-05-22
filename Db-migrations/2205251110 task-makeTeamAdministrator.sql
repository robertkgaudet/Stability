  UPDATE UserOrganization
SET IsTeamAdministrator = 1
WHERE UserId IN (
    SELECT UserId
    FROM aspnet_UsersInRoles
    WHERE RoleId = 'C0576BB3-23C2-4D43-9C42-23EF884A49A0'
);