USE Market
GO

/****** Object:  Table dbo.tbAdjustment    Script Date: 04.03.2023 13:04:39 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE dbo.Adjustment(
	Adj_Id int IDENTITY(1,1) PRIMARY KEY NOT NULL,
	Adj_Product int NOT NULL,
	Adj_Qty int DEFAULT (0) NOT NULL,
	Adj_Remarks varchar(50) NULL,
	Adj_Date date DEFAULT(GETDATE()) NOT NULL,
	Adj_User varchar(50) NULL,
) 
GO

ALTER TABLE dbo.Adjustment
  ADD FOREIGN KEY (Adj_Product)
  REFERENCES Product(Pr_Id)
