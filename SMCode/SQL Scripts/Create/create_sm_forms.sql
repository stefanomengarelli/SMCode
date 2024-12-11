/* 
	File: create_sm_repository.sql
	Date: 28-11-2024
*/

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[sm_forms](
	[IdForm] [varchar](32) NOT NULL,
	[FormType] [varchar](32) NULL,
	[Text] [varchar](255) NULL,
	[Icon] [varchar](255) NULL,
	[Enabled] [bit] NULL,
	[Debugger] [bit] NULL,
	[TableName] [varchar](255) NULL,
	[Parameters] [text] NULL,
	[Opening] [datetime] NULL,
	[Expiration] [datetime] NULL,
	[Note] [text] NULL,
	[Version] [int] NULL,
	[Deleted] [bit] NULL,
	[OnCancel] [text] NULL,
	[OnDelete] [text] NULL,
	[OnEdit] [text] NULL,
	[OnInsert] [text] NULL,
	[OnPost] [text] NULL,
	[OnReadOnly] [text] NULL,
	[OnValidate] [text] NULL,
	[FN_OnCancel] [text] NULL,
	[FN_OnDelete] [text] NULL,
	[FN_OnEdit] [text] NULL,
	[FN_OnInsert] [text] NULL,
	[FN_OnPost] [text] NULL,
	[FN_OnReadOnly] [text] NULL,
	[FN_OnValidate] [text] NULL,
	[SP_OnCancel] [text] NULL,
	[SP_OnDelete] [text] NULL,
	[SP_OnEdit] [text] NULL,
	[SP_OnInsert] [text] NULL,
	[SP_OnPost] [text] NULL,
	[SP_OnReadOnly] [text] NULL,
	[SP_OnValidate] [text] NULL,
 CONSTRAINT [PK_sm_repository] PRIMARY KEY CLUSTERED 
(
	[IdForm] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

SET ANSI_PADDING ON
GO

/****** Object:  Index [IX_sm_repository]    Script Date: 28/11/2024 10:25:57 ******/
CREATE NONCLUSTERED INDEX [IX_sm_repository] ON [dbo].[sm_repository]
(
	[Text] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO


