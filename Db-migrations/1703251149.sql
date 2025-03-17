

/****** Object:  Table [dbo].[MigrationLog]    Script Date: 3/17/2025 12:19:20 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MigrationLog](
	[MigrationLogId] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[ScriptName] [varchar](255) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_MigrationLogId] PRIMARY KEY CLUSTERED 
(
	[MigrationLogId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[MigrationLog] ADD  CONSTRAINT [DF__Migration__Migra__2CE88C3E]  DEFAULT (newid()) FOR [MigrationLogId]
GO

ALTER TABLE [dbo].[MigrationLog] ADD  CONSTRAINT [DF__Migration__Creat__2DDCB077]  DEFAULT (getdate()) FOR [CreatedOn]
GO

