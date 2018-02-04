USE [MY_PHOTO]
GO
/****** Object:  Table [dbo].[А_Города]    Script Date: 09/13/2007 00:20:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Albums] (
	[id] [int] IDENTITY (1, 1) NOT NULL ,
	[name] [varchar] (20) COLLATE Cyrillic_General_CI_AS NOT NULL ,
	[desc] [varchar] (200) COLLATE Cyrillic_General_CI_AS NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Photos] (
	[id] [int] IDENTITY (1, 1) NOT NULL ,
	[name] [varchar] (50) COLLATE Cyrillic_General_CI_AS NOT NULL ,
	[desc] [varchar] (200) COLLATE Cyrillic_General_CI_AS NULL ,
	[album_id] [int] NOT NULL ,
	[photo] [image] NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE PROC sp_GetPhotoAlbums AS
SELECT Albums.[id] AS AlbumID, Albums.[name] AS Album, Albums.[desc] AS Album_Desc,
	Photos.[id] AS PhotoID, Photos.[name] AS Photo, Photos.photo, Photos.[desc] AS Photo_Desc
FROM Albums INNER JOIN
Photos ON Albums.[id] = Photos.album_id
ORDER BY Albums.[id]
GO

CREATE PROCEDURE sp_InsertPhoto
	@name AS VARCHAR(50),
	@image AS IMAGE,
	@album AS INT
 AS

INSERT INTO Photos ([name],  photo, album_id) 
VALUES (@name, @image, @album) 

RETURN @@identity

GO

CREATE PROCEDURE sp_NewAlbum
	@name AS VARCHAR(20)
 AS

INSERT INTO Albums ([name]) 
VALUES (@name) 

RETURN @@identity
GO

