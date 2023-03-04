USE Market
GO

/****** Object:  Table dbo.Category    Script Date: 04.03.2023 13:17:14 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE dbo.Category(
	Catg_Id int IDENTITY(1,1) PRIMARY KEY NOT NULL,
	Catg_Title varchar(50) NOT NULL,
  Catg_Description text NULL
)
GO


