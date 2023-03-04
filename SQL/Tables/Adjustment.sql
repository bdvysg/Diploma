USE Market
GO

/****** Object:  Table dbo.tbAdjustment    Script Date: 04.03.2023 13:04:39 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE dbo.Adjustment(
	Ad_Id int IDENTITY(1,1) PRIMARY KEY NOT NULL,
	Ad_Referenceno varchar(50) NULL,
	Ad_Pcode varchar(50) NULL,
	Ad_Qty int NULL,
	Ad_Action varchar(50) NULL,
	Ad_Remarks varchar(50) NULL,
	Ad_Date date NULL,
	Ad_User varchar(50) NULL,
) 
GO


