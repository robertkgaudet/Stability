USE [DB_8013_staging]
GO
/****** Object:  Table [dbo].[InvitedMembers]    Script Date: 11/04/2025 9:15:20 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InvitedMembers](
	[InvitedMemberId] [uniqueidentifier] NOT NULL,
	[EmailAddress] [varchar](200) NOT NULL,
	[IsMember] [bit] NULL,
	[IsCancelled] [bit] NULL,
	[CreatedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_InvitedMembers] PRIMARY KEY CLUSTERED 
(
	[InvitedMemberId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[InvitedMembers] ADD  CONSTRAINT [DF_InvitedMembers_InvitedMemberId]  DEFAULT (newid()) FOR [InvitedMemberId]
GO
