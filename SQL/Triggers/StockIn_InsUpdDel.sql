CREATE OR ALTER TRIGGER StockIn_InsUpdDel
  ON StockIn
AFTER 
  INSERT,
  UPDATE,
  DELETE
AS
BEGIN
  IF EXISTS(SELECT * FROM inserted)
    BEGIN
      UPDATE OnStock
      SET OnStk_Quantity = OnStk_Quantity + Sip_Quantity
      FROM OnStock
      INNER JOIN StockInProduct ON Sip_Product = OnStk_Product
      INNER JOIN inserted i ON Sip_Doc = Sti_Id
      WHERE OnStk_Product = Sip_Product AND Sti_IsConfirmed = 1
    END
END
  