ALTER TABLE [DB_8013_staging].[dbo].[Profile]
ADD [ReceiveSMSNotifications] BIT NOT NULL DEFAULT(0),
    [ReceiveEmailNotifications] BIT NOT NULL DEFAULT(0);