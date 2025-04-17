ALTER TABLE [dbo].[Profile]
ADD [ReceiveSMSNotifications] BIT NOT NULL DEFAULT(0),
[ReceiveEmailNotifications] BIT NOT NULL DEFAULT(0);