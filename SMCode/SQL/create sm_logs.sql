USE [smcode]
GO

/****** Object:  Table [dbo].[sm_logs]    Script Date: 10/10/2024 10:46:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[sm_logs](
	[IdLog] [int] IDENTITY(1,1) NOT NULL,
	[UidLog] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[DateTime] [datetime] NULL,
	[IdUser] [int] NULL,
	[UidUser] [uniqueidentifier] NULL,
	[Type] [varchar](16) NULL,
	[Message] [varchar](255) NULL,
	[Details] [text] NULL,
 CONSTRAINT [PK_sm_logs] PRIMARY KEY CLUSTERED 
(
	[IdLog] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[sm_logs] ADD  CONSTRAINT [DF_sm_logs_UidLog]  DEFAULT (newid()) FOR [UidLog]
GO