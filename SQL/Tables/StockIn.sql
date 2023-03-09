USE Market
GO

/****** Object:  Table dbo.StockIn    Script Date: 04.03.2023 13:40:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE dbo.StockIn(
	Sti_Id int IDENTITY(1,1) PRIMARY KEY NOT NULL,
	Sti_Date datetime NULL,
	Sti_StockInBy varchar(50) NULL,
	Sti_Status int DEFAULT(1) NOT NULL,
	Sti_SupplierId int NULL,
  Sti_IsConfirmed bit DEFAULT(0) NOT NULL

  FOREIGN KEY(Sti_SupplierId) REFERENCES dbo.Supplier (Sup_Id),
  FOREIGN KEY(Sti_Status) REFERENCES dbo.StockInStatus (Sis_Id)
)
GO