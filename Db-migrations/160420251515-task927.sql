CREATE TABLE EmailTemplate (
    EmailTemplateId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    OrganizationId UNIQUEIDENTIFIER,  -- Matches the Organization table's uniqueidentifier type
    EmailBody NVARCHAR(MAX),
    CC NVARCHAR(500),
    BCC NVARCHAR(500),
    CONSTRAINT FK_EmailTemplate_Organization FOREIGN KEY (OrganizationId)
        REFERENCES Organization(OrganizationId)
);