CREATE TABLE OnStock (
  OnStk_Product int NOT NULL,
  OnStk_Quantity int NULL
)

ALTER TABLE OnStock
  ADD FOREIGN KEY (OnStk_Product)
  REFERENCES Product(Pr_Id)