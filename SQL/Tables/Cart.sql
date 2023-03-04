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
	Crt_Pcode varchar(50) NULL,
	Crt_Price decimal(18, 2) NULL,
	Crt_Qty int NULL,
	Crt_Disc_percent decimal(18, 2) NULL,
	Crt_Discount decimal(18, 2) NULL,
	Crt_Total decimal(18, 2) NULL,
	Crt_Date date NULL,
	Crt_Status varchar(50) NULL,
	Crt_Cashier varchar(50) NULL,
)
GO

ALTER TABLE dbo.Cart ADD  CONSTRAINT DF_Cart_Qty  DEFAULT ((0)) FOR qty
GO

ALTER TABLE dbo.Cart ADD  CONSTRAINT DF_Cart_Disc_percent  DEFAULT ((0)) FOR disc_percent
GO

ALTER TABLE dbo.Cart ADD  CONSTRAINT DF_Cart_Disc  DEFAULT ((0)) FOR disc
GO

ALTER TABLE dbo.Cart ADD  CONSTRAINT DF_Cart_Status  DEFAULT ('Pending') FOR status
GO

ALTER TABLE dbo.Cart  WITH CHECK ADD  CONSTRAINT FK_Cart_tbProduct FOREIGN KEY(pcode)
REFERENCES dbo.tbProduct (pcode)
GO

ALTER TABLE dbo.Cart CHECK CONSTRAINT FK_Cart_tbProduct
GO


