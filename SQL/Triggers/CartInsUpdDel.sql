CREATE OR ALTER TRIGGER CartInsUpdDel
ON Cart
AFTER INSERT,
      UPDATE,
      DELETE
AS
BEGIN
  IF EXISTS(SELECT * FROM inserted)
  BEGIN
    UPDATE Cart 
    SET Crt_Discount = (Crt_Price * (Crt_Disc_percent / 100)),
        Crt_Total = (Crt_Price - (Crt_Price * (Crt_Disc_percent / 100))) * Crt_Qty,
        Crt_Date = GETDATE()
    FROM Cart 
    WHERE Crt_Id IN (SELECT Crt_Id FROM inserted)
  END
END