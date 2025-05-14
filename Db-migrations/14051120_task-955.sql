 ALTER TABLE [dbo].[UserOrganization]
ADD 
    IsPreviousOwner BIT NOT NULL DEFAULT 0,
    IsOwner BIT NOT NULL DEFAULT 0;