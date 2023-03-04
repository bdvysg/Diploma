USE Market
GO

/****** Object:  Table dbo.Supplier    Script Date: 04.03.2023 13:23:10 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE dbo.Supplier(
	Sup_Id int IDENTITY(1,1) PRIMARY KEY NOT NULL,
	Sup_Name varchar(50) NOT NULL,
	Sup_Address text NOT NULL,
	Sup_ContactPerson varchar(50) NOT NULL,
	Sup_Phone varchar(50) NOT NULL,
	Sup_Email varchar(50) NOT NULL,
	Sup_Fax varchar(50) NULL,
)
GO


