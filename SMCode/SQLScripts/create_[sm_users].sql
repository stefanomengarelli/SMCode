/****** Object:  Table [dbo].[sm_users]    Script Date: 02/04/2025 10:09:46 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[sm_users](
	[IdUser] [int] IDENTITY(1,1) NOT NULL,
	[UidUser] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[UserName] [varchar](128) NOT NULL,
	[Text] [varchar](128) NULL,
	[FirstName] [varchar](128) NULL,
	[LastName] [varchar](128) NULL,
	[Email] [varchar](255) NULL,
	[Password] [varchar](255) NULL,
	[Pin] [int] NULL,
	[Register] [int] NULL,
	[TaxCode] [varchar](16) NULL,
	[BirthDate] [datetime] NULL,
	[Sex] [varchar](1) NULL,
	[Icon] [varchar](255) NULL,
	[Image] [varchar](255) NULL,
	[Host] [varchar](128) NULL,
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

SET ANSI_PADDING ON
GO

/****** Object:  Index [IX_sm_users]    Script Date: 02/04/2025 10:09:46 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_sm_users] ON [dbo].[sm_users]
(
	[UserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

ALTER TABLE [dbo].[sm_users] ADD  CONSTRAINT [DF_sm_users_UidUser]  DEFAULT (newid()) FOR [UidUser]
GO

ALTER TABLE [dbo].[sm_users] ADD  CONSTRAINT [DF_sm_users_InsertionDate]  DEFAULT (getdate()) FOR [InsertionDate]
GO
