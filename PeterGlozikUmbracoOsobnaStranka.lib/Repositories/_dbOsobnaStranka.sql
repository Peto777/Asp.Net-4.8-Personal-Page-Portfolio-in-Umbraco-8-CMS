/****** Object:  Table [dbo].[osUserProp] ******/
CREATE TABLE [dbo].[osUserProp](
	[pk] [uniqueidentifier] NOT NULL,
	[dateCreate] [datetime] NOT NULL,
	[userId] [int] NULL,
	[sessionId] [nvarchar](255) NULL,
	[propId] [nvarchar](255) NOT NULL,
	[propValue] [ntext] NULL,
 CONSTRAINT [PK_osUserProp] PRIMARY KEY CLUSTERED 
(
	[pk] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


/****** Object:  Table [dbo].[osBlogPost] ******/
	CREATE TABLE [dbo].[osBlogPost](
		[pk] [uniqueidentifier] NOT NULL,
		[blogName] [nvarchar](255) NOT NULL,
		[blogDescription] [ntext] NULL,
		[blogWeb] [nvarchar](255) NULL,
		[blogImagePath] [nvarchar](255) NULL,
	 CONSTRAINT [PK_osBlogPost] PRIMARY KEY CLUSTERED 
	(
		[pk] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	GO

/****** Object:  Table [dbo].[osCountry] ******/
CREATE TABLE [dbo].[osCountry](
	[pk] [uniqueidentifier] NOT NULL,
	[code] [nvarchar](50) NOT NULL,
	[name] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_osCountry] PRIMARY KEY CLUSTERED 
(
	[pk] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


