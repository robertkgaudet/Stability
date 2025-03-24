-- Adding a new boolean field [EnableTeamMemberVerification] to the Organization table

ALTER TABLE [DB_8013_staging].[dbo].[Organization]
ADD [EnableTeamMemberVerification] BIT DEFAULT 0;