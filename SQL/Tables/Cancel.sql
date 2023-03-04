USE Market
GO

/****** Object:  Table dbo.tbCancel    Script Date: 04.03.2023 13:08:14 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE dbo.Cancel(
	Cn_Id int IDENTITY(1,1) PRIMARY KEY NOT NULL,
	Cn_Transno varchar(50) NULL,
	Cn_Product int NOT NULL,
	Cn_Price decimal(18, 2) NULL,
	Cn_Qty int NULL,
	Cn_Total decimal(18, 2) NULL,
	Cn_Date date NULL,
	Cn_Voidby varchar(50) NULL,
	Cn_Cancelledby varchar(50) NULL,
	Cn_reason text NULL,
	Cn_Action varchar(50) NULL,
)
GO

ALTER TABLE Cancel
  ADD FOREIGN KEY (Cn_Product)
  REFERENCES Product(Pr_Id)
