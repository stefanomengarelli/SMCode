/* 
	File: create_sm_users_organizations.sql
	Date: 31-10-2024
*/

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[sm_users_organizations](
	[IdUserOrganization] [int] IDENTITY(1,1) NOT NULL,
	[IdUser] [int] NOT NULL,
	[IdOrganization] [int] NOT NULL,
	[Deleted] [bit] NULL,
	[InsertionDate] [datetime] NULL,
	[InsertionUser] [varchar](16) NULL,
	[ModificationDate] [datetime] NULL,
	[ModificationUser] [varchar](16) NULL,
	[DeletionDate] [datetime] NULL,
	[DeletionUser] [varchar](16) NULL,
 CONSTRAINT [PK_sm_users_organizations] PRIMARY KEY CLUSTERED 
(
	[IdUserOrganization] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Index [IX_sm_users_organizations]    Script Date: 31/10/2024 11:56:31 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_sm_users_organizations] ON [dbo].[sm_users_organizations]
(
	[IdUser] ASC,
	[IdOrganization] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

ALTER TABLE [dbo].[sm_users_organizations] ADD  CONSTRAINT [DF_sm_users_organizations_InsertionDate]  DEFAULT (getdate()) FOR [InsertionDate]
GO
