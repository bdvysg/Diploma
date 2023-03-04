USE Market
GO

/****** Object:  Table dbo.User    Script Date: 04.03.2023 13:54:42 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE dbo.[User](
  Usr_Id int IDENTITY(1, 1) PRIMARY KEY NOT NULL,
  Usr_Employee int NOT NULL,
	Usr_Username varchar(50) NOT NULL,
	Usr_Password varchar(50) NOT NULL,
	Usr_Role int NOT NULL,
	Usr_IsActivate bit DEFAULT(0) NOT NULL,
)
GO

ALTER TABLE [dbo].[User]
  ADD FOREIGN KEY (Usr_Role)
  REFERENCES dbo.UserRole(Ur_Id)
  GO

ALTER TABLE [dbo].[User]
  ADD FOREIGN KEY (Usr_Employee)
  REFERENCES dbo.Employee(Emp_Id)
  GO


