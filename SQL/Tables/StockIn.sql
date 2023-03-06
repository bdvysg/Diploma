USE Market
GO

/****** Object:  Table dbo.StockIn    Script Date: 04.03.2023 13:40:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE dbo.StockIn(
	Sti_id int IDENTITY(1,1) PRIMARY KEY NOT NULL,
	Sti_Refno varchar(50) NULL,
	Sti_Product int NOT NULL,
	Sti_Qty int NULL,
	Sti_Date datetime NULL,
	Sti_StockInBy varchar(50) NULL,
	Sti_Status int DEFAULT(1) NOT NULL,
	Sti_SupplierId int NULL,
)
GO

ALTER TABLE dbo.StockIn ADD  CONSTRAINT DF_StockIn_qty  DEFAULT ((0)) FOR Sti_qty
GO


ALTER TABLE dbo.StockIn  WITH CHECK ADD  CONSTRAINT FK_StockIn_Product FOREIGN KEY(Sti_Product)
REFERENCES dbo.Product (Pr_Id)
GO

ALTER TABLE dbo.StockIn CHECK CONSTRAINT FK_StockIn_Product
GO

ALTER TABLE dbo.StockIn  WITH CHECK ADD  CONSTRAINT FK_StockIn_Supplier FOREIGN KEY(Sti_SupplierId)
REFERENCES dbo.tbSupplier (Sup_Id)
GO

ALTER TABLE dbo.StockIn CHECK CONSTRAINT FK_StockIn_Supplier
GO

ALTER TABLE dbo.StockIn  WITH CHECK ADD  CONSTRAINT FK_StockIn_StockInStatus FOREIGN KEY(Sti_Status)
REFERENCES dbo.StockInStatus (Sti_Id)
GO

ALTER TABLE dbo.StockIn CHECK CONSTRAINT FK_StockIn_StockInStatus
GO
