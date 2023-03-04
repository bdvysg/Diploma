USE Market
GO

/****** Object:  Table dbo.tbStore    Script Date: 04.03.2023 13:20:21 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE dbo.Store(
  Str_Id int PRIMARY KEY NOT NULL,
	Str_Title varchar(50) NOT NULL,
	address varchar(max) NULL
) 
GO


