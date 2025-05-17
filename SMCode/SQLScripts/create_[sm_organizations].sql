/****** Object:  Table [dbo].[sm_organizations]    Script Date: 02/04/2025 09:57:51 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[sm_organizations](
	[IdOrganization] [int] NOT NULL,
	[UidOrganization] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[Text] [varchar](128) NULL,
	[Icon] [varchar](255) NULL,
	[Image] [varchar](255) NULL,
	[Parameters] [text] NULL,
	[ByDefault] [bit] NULL,
	[Deleted] [bit] NULL,
	[InsertionDate] [datetime] NULL,
	[InsertionUser] [varchar](16) NULL,
	[ModificationDate] [datetime] NULL,
	[ModificationUser] [varchar](16) NULL,
	[DeletionDate] [datetime] NULL,
	[DeletionUser] [varchar](16) NULL,
 CONSTRAINT [PK_sm_organizations] PRIMARY KEY CLUSTERED 
(
	[IdOrganization] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[sm_organizations] ADD  CONSTRAINT [DF_sm_organizations_UidOrganization]  DEFAULT (newid()) FOR [UidOrganization]
GO

ALTER TABLE [dbo].[sm_organizations] ADD  CONSTRAINT [DF_sm_organizations_InsertionDate]  DEFAULT (getdate()) FOR [InsertionDate]
GO
