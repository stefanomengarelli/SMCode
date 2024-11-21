/* 
	File: create_sm_controls.sql
	Date: 21-11-2024
*/

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[sm_controls](
	[IdControl] [int] IDENTITY(1,1) NOT NULL,
	[UidControl] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[IdForm] [varchar](16) NULL,
	[Alias] [varchar](32) NULL,
	[ControlType] [varchar](16) NULL,
	[ViewIndex] [int] NULL,
	[Text] [text] NULL,
	[ShortText] [varchar](50) NULL,
	[Format] [varchar](50) NULL,
	[Length] [int] NULL,
	[GridColumns] [int] NULL,
	[Required] [bit] NULL,
	[ColumnName] [varchar](64) NULL,
	[ColumnView] [int] NULL,
	[ColumnAPI] [varchar](64) NULL,
	[ColumnExport] [varchar](64) NULL,
	[TableName] [varchar](64) NULL,
	[Parameters] [text] NULL,
	[Options] [text] NULL,
	[OnChange] [text] NULL,
	[OnEnable] [text] NULL,
	[OnFocus] [text] NULL,
	[OnInitialize] [text] NULL,
	[OnUpdate] [text] NULL,
	[OnValidate] [text] NULL,
	[OnVisible] [text] NULL,
	[Debugger] [bit] NULL,
	[Note] [text] NULL,
	[Version] [int] NULL,
	[Deleted] [bit] NULL,
	[InsertionDate] [datetime] NULL,
	[InsertionUser] [varchar](16) NULL,
	[ModificationDate] [datetime] NULL,
	[ModificationUser] [varchar](16) NULL,
	[DeletionDate] [datetime] NULL,
	[DeletionUser] [varchar](16) NULL,
 CONSTRAINT [PK_sm_controls] PRIMARY KEY CLUSTERED 
(
	[IdControl] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

SET ANSI_PADDING ON
GO

/****** Object:  Index [IX_sm_controls_alias]    Script Date: 21/11/2024 15:48:56 ******/
CREATE NONCLUSTERED INDEX [IX_sm_controls_alias] ON [dbo].[sm_controls]
(
	[Alias] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

SET ANSI_PADDING ON
GO

/****** Object:  Index [IX_sm_controls_viewindex]    Script Date: 21/11/2024 15:48:56 ******/
CREATE NONCLUSTERED INDEX [IX_sm_controls_viewindex] ON [dbo].[sm_controls]
(
	[IdForm] ASC,
	[ViewIndex] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

ALTER TABLE [dbo].[sm_controls] ADD  CONSTRAINT [DF_sm_controls_UidControl]  DEFAULT (newid()) FOR [UidControl]
GO

ALTER TABLE [dbo].[sm_controls] ADD  CONSTRAINT [DF_sm_controls_InsertionDate]  DEFAULT (getdate()) FOR [InsertionDate]
GO


