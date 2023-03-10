CREATE TABLE OnStock (
  OnStk_Product int NOT NULL,
  OnStk_Quantity int DEFAULT(0) NOT NULL
)

ALTER TABLE OnStock
  ADD FOREIGN KEY (OnStk_Product)
  REFERENCES Product(Pr_Id)

ALTER TABLE OnStock ADD DEFAULT (0) FOR OnStk_Quantity