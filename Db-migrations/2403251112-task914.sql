-- Adding a new boolean field [EnableTeamMemberVerification] to the Organization table

ALTER TABLE .[dbo].[Organization]
ADD [EnableTeamMemberVerification] BIT DEFAULT 0;