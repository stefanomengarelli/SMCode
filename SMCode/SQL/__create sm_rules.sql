USE [smcode]
GO

/****** Object:  Table [dbo].[sm_rules]    Script Date: 10/10/2024 15:40:02 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[sm_rules](
	[IdRule] [int] IDENTITY(1,1) NOT NULL,
	[UidRule] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[Caption] [varchar](128) NULL,
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
 CONSTRAINT [PK_sm_rules] PRIMARY KEY CLUSTERED 
(
	[IdRule] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[sm_rules] ADD  CONSTRAINT [DF_sm_rules_UidRule]  DEFAULT (newid()) FOR [UidRule]
GO

ALTER TABLE [dbo].[sm_rules] ADD  CONSTRAINT [DF_sm_rules_InsertionDate]  DEFAULT (getdate()) FOR [InsertionDate]
GO


