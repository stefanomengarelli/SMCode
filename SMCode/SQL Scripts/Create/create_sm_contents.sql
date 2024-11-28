/* 
	File: create_sm_contents.sql
	Date: 28-11-2024
*/

/****** Object:  Table [dbo].[sm_contents]    Script Date: 28/11/2024 10:53:44 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[sm_contents](
	[IdContents] [int] IDENTITY(1,1) NOT NULL,
	[IdDocument] [int] NULL,
	[IdForm] [varchar](32) NULL,
	[IdControl] [int] NULL,
	[ValueIndex] [int] NULL,
	[Value] [text] NULL,
	[Deleted] [bit] NULL,
 CONSTRAINT [PK_sm_contents] PRIMARY KEY CLUSTERED 
(
	[IdContents] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

SET ANSI_PADDING ON
GO

/****** Object:  Index [IX_sm_contents]    Script Date: 28/11/2024 10:53:44 ******/
CREATE NONCLUSTERED INDEX [IX_sm_contents] ON [dbo].[sm_contents]
(
	[IdDocument] ASC,
	[IdForm] ASC,
	[IdControl] ASC,
	[ValueIndex] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO


