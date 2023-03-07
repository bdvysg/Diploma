USE Market
GO

/****** Object:  Table dbo.Product    Script Date: 04.03.2023 13:26:16 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE dbo.Product(
	Pr_Id int IDENTITY(1, 1) PRIMARY KEY NOT NULL,
	Pr_BarCode varchar(20) NULL,
	Pr_Description varchar(max) NOT NULL,
	Pr_Brand int NOT NULL,
  Pr_Title varchar(200),
	Pr_Category int NOT NULL,
	Pr_PriceOpt decimal(18, 2) NOT NULL,
  Pr_Price decimal(18, 2) NOT NULL,
	Pr_Qty varchar(20) NULL,
	Pr_Reorder int NULL,
  Pr_Image varbinary(MAX) NULL
)
GO

ALTER TABLE dbo.Product ADD  FOREIGN KEY(Pr_Brand)
REFERENCES dbo.Brand (Br_Id)
GO

ALTER TABLE dbo.Product ADD FOREIGN KEY(Pr_Category)
REFERENCES dbo.Category (Catg_Id)
GO
