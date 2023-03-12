CREATE TRIGGER CartInsUpdDel
ON Cart
AFTER INSERT,
      UPDATE,
      DELETE
AS
BEGIN
  IF EXISTS(SELECT * FROM inserted)
  BEGIN
    UPDATE Cart
    SET Crt_Total = (Crt_Price - Crt_Price * Crt_Discount) * Crt_Qty
  END
END