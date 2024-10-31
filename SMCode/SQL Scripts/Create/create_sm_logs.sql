/* 
	File: create_sm_logs.sql
	Date: 31-10-2024
*/

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[sm_logs](
	[IdLog] [int] IDENTITY(1,1) NOT NULL,
	[UidLog] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[DateTime] [datetime] NULL,
	[Application] [varchar](64) NULL,
	[Version] [varchar](64) NULL,
	[IdUser] [int] NULL,
	[UidUser] [uniqueidentifier] NULL,
	[LogType] [varchar](16) NULL,
	[Action] [varchar](64) NULL,
	[Message] [varchar](255) NULL,
	[Details] [text] NULL,
 CONSTRAINT [PK_sm_logs] PRIMARY KEY CLUSTERED 
(
	[IdLog] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

SET ANSI_PADDING ON
GO

/****** Object:  Index [IX_sm_logs]    Script Date: 31/10/2024 11:41:39 ******/
CREATE NONCLUSTERED INDEX [IX_sm_logs] ON [dbo].[sm_logs]
(
	[Application] ASC,
	[Action] ASC,
	[IdLog] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

ALTER TABLE [dbo].[sm_logs] ADD  CONSTRAINT [DF_sm_logs_UidLog]  DEFAULT (newid()) FOR [UidLog]
GO

ALTER TABLE [dbo].[sm_logs] ADD  CONSTRAINT [DF_sm_logs_DateTime]  DEFAULT (getdate()) FOR [DateTime]
GO
