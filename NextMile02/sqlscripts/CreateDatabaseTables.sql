If NOT EXISTS (SELECT 1 FROM SysDatabases WHERE NAME = 'NextMile02')
	CREATE DATABASE [NextMile02]
GO
USE [NextMile02]
GO

/****** Object:  Table [dbo].[UserProfileTest1]    Script Date: 12/10/2015 7:17:38 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
If NOT EXISTS (SELECT 1 FROM Information_Schema.Tables WHERE Table_NAME = 'UserProfileTest1')
	CREATE TABLE [dbo].[UserProfileTest1](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[userid] [nvarchar](60) NOT NULL,
		[truckname] [nvarchar](100) NOT NULL,
		[preference] [int] NULL,
	PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
GO
If NOT EXISTS (SELECT 1 FROM Information_Schema.Tables WHERE Table_NAME = 'UserDetails')
	CREATE TABLE [dbo].[UserDetails](
		[fld_user_id] [bigint] IDENTITY(1,1) NOT NULL,
		[fld_facebook_id] [varchar](100) NOT NULL,
		[fld_user_name] [varchar](60) NOT NULL,
		[fld_user_email] [varchar](60) NULL,
		[fld_user_doj] [int] NOT NULL,
	PRIMARY KEY CLUSTERED 
	(
		[fld_user_id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
GO
