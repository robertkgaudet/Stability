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
