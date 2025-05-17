/****** Object:  Table [dbo].[sm_users_organizations]    Script Date: 02/04/2025 10:17:14 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[sm_users_organizations](
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
	[IdUser] ASC,
	[IdOrganization] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[sm_users_organizations] ADD  CONSTRAINT [DF_sm_users_organizations_InsertionDate]  DEFAULT (getdate()) FOR [InsertionDate]
GO
