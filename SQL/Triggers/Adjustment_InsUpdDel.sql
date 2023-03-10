CREATE OR ALTER TRIGGER Adjustment_InsUpdDel
  ON Adjustment
AFTER 
  INSERT,
  UPDATE,
  DELETE
AS
BEGIN
  IF EXISTS(SELECT * FROM inserted)
    BEGIN
      UPDATE OnStock
      SET OnStk_Quantity = OnStk_Quantity - Adj_Qty
      FROM OnStock
      INNER JOIN inserted i
        ON i.Adj_Product = OnStk_Product
      WHERE OnStk_Product = Adj_Product
    END
END
  