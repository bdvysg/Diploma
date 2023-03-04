USE Market
GO

/****** Object:  Table dbo.Cart    Script Date: 04.03.2023 13:11:27 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE dbo.Cart(
	Crt_Id int IDENTITY(1,1) PRIMARY KEY NOT NULL,
	Crt_Transno varchar(50) NULL,
	Crt_Product int NOT NULL,
	Crt_Price decimal(18, 2) NULL,
	Crt_Qty int NULL,
	Crt_Disc_percent decimal(18, 2) NULL,
	Crt_Discount decimal(18, 2) NULL,
	Crt_Total decimal(18, 2) NULL,
	Crt_Date date NULL,
	Crt_Status int DEFAULT(1) NOT NULL,
	Crt_Cashier int NULL,
)
GO

ALTER TABLE dbo.Cart ADD DEFAULT ((0)) FOR Crt_Qty
GO

ALTER TABLE dbo.Cart ADD DEFAULT ((0)) FOR Crt_Disc_percent
GO

ALTER TABLE dbo.Cart ADD DEFAULT ((0)) FOR Crt_Discount

ALTER TABLE dbo.Cart 
  ADD FOREIGN KEY(Crt_Product)
  REFERENCES dbo.Product (Pr_Id)
  GO

ALTER TABLE dbo.Cart
  ADD FOREIGN KEY (Crt_Cashier)
  REFERENCES dbo.Employee (Emp_Id)