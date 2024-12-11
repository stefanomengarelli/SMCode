/* 
	File: create_sm_users_rules.sql
	Date: 31-10-2024
*/

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[sm_users_rules](
	[IdUserRule] [int] IDENTITY(1,1) NOT NULL,
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
	[IdUserRule] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Index [IX_sm_users_rules]    Script Date: 31/10/2024 11:52:28 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_sm_users_rules] ON [dbo].[sm_users_rules]
(
	[IdUser] ASC,
	[IdRule] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

ALTER TABLE [dbo].[sm_users_rules] ADD  CONSTRAINT [DF_sm_users_rules_InsertionDate]  DEFAULT (getdate()) FOR [InsertionDate]
GO
