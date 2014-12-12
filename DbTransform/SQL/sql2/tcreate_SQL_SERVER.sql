
CREATE TABLE [dbo].[T_CountriesPopulation](
	[Rank] [int] NULL,
	[Country] [nvarchar](200) NULL,
	[Country_Official] [nvarchar](200) NULL,
	[Population] [int] NULL,
	[Date] [nvarchar](50) NULL,
	[PercentOfWorldPopulation] [decimal](20, 6) NULL,
	[Source] [nvarchar](100) NULL
) ON [PRIMARY]

GO





CREATE TABLE [dbo].[T_MAP_CountriesContinent](
	[Id] [int] NULL,
	[Continent] [nvarchar](20) NULL,
	[MainShell] [nvarchar](20) NULL,
	[Name] [nvarchar](100) NULL,
	[NameFdating] [nvarchar](100) NULL,
	[NamePopulation] [nchar](200) NULL,
	[Transcontinental] [nvarchar](20) NULL,
	[PartialContinent] [nvarchar](100) NULL,
	[Capital] [nvarchar](1000) NULL,
	[Status] [nvarchar](1000) NULL
) ON [PRIMARY]

GO



CREATE TABLE [dbo].[T_MAP_ProfilePhoto](
	[MAP_PRFPhoto_UID] [uniqueidentifier] NULL,
	[MAP_PRFPhoto_ProfileId] [int] NULL,
	[MAP_PRFPhoto_UploadName] [nvarchar](1001) NULL,
	[MAP_PRFPhoto_Mime] [nchar](10) NULL,
	[MAP_PRFPhoto_Added] [datetime] NULL,
	[MAP_PRFPhoto_Path] [nvarchar](2000) NULL,
	[MAP_PRFPhoto_IsDefault] [bit] NULL,
	[MAP_PRFPhoto_Status] [int] NULL
) ON [PRIMARY]

GO




CREATE TABLE [dbo].[IP2country](
	[IP_Block_Start] [bigint] NOT NULL,
	[IP_Block_End] [bigint] NULL,
	[BlockStart] [varchar](20) NULL,
	[BlockEnd] [varchar](20) NULL,
	[IP_Block_Start_Upper] [bigint] NULL,
	[IP_Block_Start_Lower] [bigint] NULL,
	[IP_Block_End_Upper] [bigint] NULL,
	[IP_Block_End_Lower] [bigint] NULL,
	[TwoLetter] [nchar](2) NULL,
	[ThreeLetter] [nchar](3) NULL,
	[Country] [nvarchar](1000) NULL,
 CONSTRAINT [PK_ip2country] PRIMARY KEY CLUSTERED 
(
	[IP_Block_Start] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO 

CREATE TABLE [dbo].[Zodiacs](
	[ZodiacSign] [nvarchar](50) NULL,
	[ZodiacSign_DE] [nvarchar](50) NULL,
	[ZodiacSign_Lat] [nvarchar](50) NULL,
	[Start] [varchar](10) NULL,
	[End] [varchar](10) NULL,
	[QuickCharacteristics] [nvarchar](100) NULL
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[SQL_Strology_Chinese](
	[Remainder] [nchar](10) NULL,
	[ChineseZodiac] [nvarchar](50) NULL
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[ChineseYears](
	[CHI_UID] [uniqueidentifier] NULL,
	[WesternYear] [int] NULL,
	[DateFrom] [datetime] NULL,
	[DateTo] [datetime] NULL
) ON [PRIMARY]

GO


CREATE TABLE [dbo].[Zodiacs](
	[ZodiacSign] [nvarchar](50) NULL,
	[ZodiacSign_DE] [nvarchar](50) NULL,
	[ZodiacSign_Lat] [nvarchar](50) NULL,
	[Start] [varchar](10) NULL,
	[End] [varchar](10) NULL,
	[QuickCharacteristics] [nvarchar](100) NULL
) ON [PRIMARY]

GO

