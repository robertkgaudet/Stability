INSERT INTO [dbo].[aspnet_Roles]
    ([ApplicationId], [RoleId], [RoleName], [LoweredRoleName], [Description])
VALUES 
    ('F2BD772B-9A05-489D-9BBF-70C149C0EC1D', '356697D6-65CA-445E-85C9-CDFA98248C47', 'Donor', 'donor', null);
	
alter table Donation add PaymentProvider int null;
alter table Donation add DonationStatus int null;
alter table Donation add ErrorDetails nvarchar(max) null;



SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DonationCampaign](
	[DonationCampaignId] [uniqueidentifier] NOT NULL,
	[OrganizationEventId] [uniqueidentifier] NULL,
	[Summary] [nvarchar](max) NULL,
	[Address] [nvarchar](max) NULL,
	[Amount] [varchar](255) NULL,
	[Description] [nvarchar](max) NULL,
	[IsDefault] [bit] not NULL,
 CONSTRAINT [PK_DonationCampaigns] PRIMARY KEY CLUSTERED 
(
	[DonationCampaignId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[DonationCampaign] ADD  DEFAULT ((0)) FOR [IsDefault]
GO




ALTER TABLE [dbo].[Donation] ALTER COLUMN [CampaignId] [uniqueidentifier] NULL;
ALTER TABLE [dbo].[Donation] ALTER COLUMN [BasicNeedsSurveyItemId] [uniqueidentifier] NULL;
ALTER TABLE [dbo].[Donation] Add DonationCampaignId [uniqueidentifier] NULL;
ALTER TABLE [dbo].[Donation]  
ADD CONSTRAINT [FK_Donation_DonationCampaign] 
FOREIGN KEY ([DonationCampaignId]) REFERENCES [dbo].[DonationCampaign] ([DonationCampaignId]);

alter table DonationCampaign
add OrganizationId [uniqueidentifier] not NULL

ALTER TABLE Donation  
ADD TransactionFee DECIMAL(10,2)

ALTER TABLE Donation   
ADD TotalAmount DECIMAL(10,2)


ALTER TABLE [dbo].[Post]
ADD [EventId] [uniqueidentifier] NULL;

ALTER TABLE [dbo].[Post]  WITH CHECK ADD  CONSTRAINT [FK_Post_Event] FOREIGN KEY([EventId])
REFERENCES [dbo].[Event] ([EventId])
GO



USE [DB_8013_stability]
GO
/****** Object:  Table [dbo].[Comment]    Script Date: 1/28/2025 8:09:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Comment](
	[CommentId] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[Comment] [varchar](max) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [uniqueidentifier] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[ParentId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_Comment] PRIMARY KEY CLUSTERED 
(
	[CommentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CommentType]    Script Date: 1/28/2025 8:09:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CommentType](
	[CommentTypeId] [uniqueidentifier] NOT NULL,
	[CommentTypeTitle] [varchar](50) NOT NULL,
 CONSTRAINT [PK_CommentType] PRIMARY KEY CLUSTERED 
(
	[CommentTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PostComment]    Script Date: 1/28/2025 8:09:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PostComment](
	[PostCommentId] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[PostId] [uniqueidentifier] NOT NULL,
	[CommentId] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_PostComment] PRIMARY KEY CLUSTERED 
(
	[PostCommentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Comment] ADD  CONSTRAINT [DF_Comment_CommentId]  DEFAULT (newid()) FOR [CommentId]
GO
ALTER TABLE [dbo].[Comment] ADD  CONSTRAINT [DF_Comment_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Comment] ADD  CONSTRAINT [DF_Comment_ParentId]  DEFAULT (newid()) FOR [ParentId]
GO
ALTER TABLE [dbo].[CommentType] ADD  CONSTRAINT [DF_CommentType_CommentTypeId]  DEFAULT (newid()) FOR [CommentTypeId]
GO
ALTER TABLE [dbo].[PostComment] ADD  CONSTRAINT [DF_PostComment_PostCommentId]  DEFAULT (newid()) FOR [PostCommentId]
GO
ALTER TABLE [dbo].[PostComment]  WITH CHECK ADD  CONSTRAINT [FK_PostComment_aspnet_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[PostComment] CHECK CONSTRAINT [FK_PostComment_aspnet_Users]
GO
ALTER TABLE [dbo].[PostComment]  WITH CHECK ADD  CONSTRAINT [FK_PostComment_Comment] FOREIGN KEY([CommentId])
REFERENCES [dbo].[Comment] ([CommentId])
GO
ALTER TABLE [dbo].[PostComment] CHECK CONSTRAINT [FK_PostComment_Comment]
GO
ALTER TABLE [dbo].[PostComment]  WITH CHECK ADD  CONSTRAINT [FK_PostComment_Post] FOREIGN KEY([PostId])
REFERENCES [dbo].[Post] ([PostId])
GO
ALTER TABLE [dbo].[PostComment] CHECK CONSTRAINT [FK_PostComment_Post]
GO

ALTER TABLE [dbo].[Post]
ADD [EventId] [uniqueidentifier] NULL;

ALTER TABLE [dbo].[Post]  WITH CHECK ADD  CONSTRAINT [FK_Post_Event] FOREIGN KEY([EventId])
REFERENCES [dbo].[Event] ([EventId])
GO








/****** Object:  Table [dbo].[FeatureType]    Script Date: 05-02-2025 19:05:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FeatureType](
	[FeatureTypeId] [int] IDENTITY(1,1) NOT NULL,
	[FeatureName] [nvarchar](100) NOT NULL,
	[RedirectURL] [nvarchar](500) NOT NULL,
	[IncludeInStream] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[FeatureTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Notification]    Script Date: 05-02-2025 19:05:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Notification](
	[NotificationId] [uniqueidentifier] NOT NULL,
	[Title] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](1000) NOT NULL,
	[RedirectURLParameters] [nvarchar](500) NULL,
	[NotificationType] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[IsRead] [bit] NOT NULL,
	[SenderUserId] [uniqueidentifier] NULL,
	[RecipientUserId] [uniqueidentifier] NULL,
	[FeatureTypeId] [int] NULL,
	[PostToStream] [bit] NULL,
 CONSTRAINT [PK_Notification] PRIMARY KEY CLUSTERED 
(
	[NotificationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[FeatureType] ON 

INSERT [dbo].[FeatureType] ([FeatureTypeId], [FeatureName], [RedirectURL], [IncludeInStream]) VALUES (1, N'Test', N'https://example.com/test', 0)
SET IDENTITY_INSERT [dbo].[FeatureType] OFF
GO


ALTER TABLE [dbo].[FeatureType] ADD  DEFAULT ((0)) FOR [IncludeInStream]
GO
ALTER TABLE [dbo].[Notification]  WITH CHECK ADD  CONSTRAINT [FK_Notification_FeatureType] FOREIGN KEY([FeatureTypeId])
REFERENCES [dbo].[FeatureType] ([FeatureTypeId])
GO
ALTER TABLE [dbo].[Notification] CHECK CONSTRAINT [FK_Notification_FeatureType]
GO


alter table ProfilePhoto
add CreatedOn datetime default GetDate()