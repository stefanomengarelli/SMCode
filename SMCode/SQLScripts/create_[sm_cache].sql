/****** Object:  Table [dbo].[sm_cache]    Script Date: 02/04/2025 10:05:21 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[sm_cache](
	[IdCache] [int] IDENTITY(1,1) NOT NULL,
	[CacheUser] [int] NULL,
	[CacheKey] [varchar](255) NULL,
	[CacheValue] [text] NULL,
	[CacheExpire] [datetime] NULL,
	[Deleted] [bit] NULL,
	[InsertionDate] [datetime] NULL,
	[InsertionUser] [varchar](16) NULL,
	[ModificationDate] [datetime] NULL,
	[ModificationUser] [varchar](16) NULL,
	[DeletionDate] [datetime] NULL,
	[DeletionUser] [varchar](16) NULL,
 CONSTRAINT [PK_sm_cache] PRIMARY KEY CLUSTERED 
(
	[IdCache] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

SET ANSI_PADDING ON
GO

/****** Object:  Index [IX_sm_cache]    Script Date: 02/04/2025 10:05:21 ******/
CREATE NONCLUSTERED INDEX [IX_sm_cache] ON [dbo].[sm_cache]
(
	[CacheUser] ASC,
	[CacheKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

ALTER TABLE [dbo].[sm_cache] ADD  CONSTRAINT [DF_sm_cache_InsertionDate]  DEFAULT (getdate()) FOR [InsertionDate]
GO
