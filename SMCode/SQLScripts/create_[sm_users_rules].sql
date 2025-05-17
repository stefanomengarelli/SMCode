/****** Object:  Table [dbo].[sm_users_rules]    Script Date: 17/05/2025 11:04:05 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[sm_users_rules](
	[IdUser] [int] NOT NULL,
	[IdRule] [int] NOT NULL,
	[Deleted] [bit] NULL,
	[InsertionDate] [datetime] NULL,
	[InsertionUser] [varchar](16) NULL,
	[ModificationDate] [datetime] NULL,
	[ModificationUser] [varchar](16) NULL,
	[DeletionDate] [datetime] NULL,
	[DeletionUser] [varchar](16) NULL,
 CONSTRAINT [PK_sm_users_rules] PRIMARY KEY CLUSTERED 
(
	[IdUser] ASC,
	[IdRule] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[sm_users_rules] ADD  CONSTRAINT [DF_sm_users_rules_InsertionDate]  DEFAULT (getdate()) FOR [InsertionDate]
GO
