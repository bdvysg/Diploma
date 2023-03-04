USE [Market]
GO

/****** Object:  Table [dbo].[tbBrand]    Script Date: 04.03.2023 12:59:15 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Brand](
	Br_Id int IDENTITY(1,1) NOT NULL,
	Br_Title varchar(50) NOT NULL,
 CONSTRAINT [PK_tbBrand] PRIMARY KEY CLUSTERED 
( 
GO


