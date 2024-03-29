CREATE TABLE StockInProduct (
  Sip_Id int IDENTITY(1, 1) PRIMARY KEY NOT NULL,
  Sip_Doc int NOT NULL,
  Sip_Product int NOT NULL,
  Sip_Quantity int DEFAULT(0) NOT NULL

  FOREIGN KEY (Sip_Product) REFERENCES Product(Pr_Id),
  FOREIGN KEY (Sip_Doc) REFERENCES StockIn(Sti_Id)
)
