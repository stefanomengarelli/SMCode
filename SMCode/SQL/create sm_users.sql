USE [smcode]
GO

/****** Object:  Table [dbo].[sm_users]    Script Date: 10/10/2024 15:18:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[sm_users](
	[IdUser] [int] IDENTITY(1,1) NOT NULL,
	[UidUser] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[UserId] [varchar](128) NOT NULL,
	[Name] [varchar](128) NULL,
	[Email] [varchar](255) NULL,
	[Password] [varchar](255) NULL,
	[Pin] [int] NULL,
	[Register] [int] NULL,
	[TaxCode] [varchar](16) NULL,
	[BirthDate] [datetime] NULL,
	[Sex] [varchar](1) NULL,
	[Icon] [varchar](255) NULL,
	[Image] [varchar](255) NULL,
	[IPAddress] [varchar](64) NULL,
	[Note] [text] NULL,
	[Deleted] [bit] NULL,
	[InsertionDate] [datetime] NULL,
	[InsertionUser] [varchar](16) NULL,
	[ModificationDate] [datetime] NULL,
	[ModificationUser] [varchar](16) NULL,
	[DeletionDate] [datetime] NULL,
	[DeletionUser] [varchar](16) NULL,
 CONSTRAINT [PK_sm_users] PRIMARY KEY CLUSTERED 
(
	[IdUser] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[sm_users] ADD  CONSTRAINT [DF_sm_users_UidUser]  DEFAULT (newid()) FOR [UidUser]
GO

ALTER TABLE [dbo].[sm_users] ADD  CONSTRAINT [DF_sm_users_InsertionDate]  DEFAULT (getdate()) FOR [InsertionDate]
GO
