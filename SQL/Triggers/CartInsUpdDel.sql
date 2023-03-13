CREATE OR ALTER TRIGGER CartInsUpdDel
ON Cart
AFTER INSERT,
      UPDATE,
      DELETE
AS
BEGIN
  IF EXISTS(SELECT * FROM inserted)
  BEGIN
  BEGIN TRAN
    UPDATE Cart 
    SET Crt_Discount = (C.Crt_Price * (C.Crt_Disc_percent / 100))
    FROM Cart C
    INNER JOIN inserted i
      ON i.Crt_Id = c.Crt_Id  
  COMMIT TRAN
    UPDATE Cart 
    SET Crt_Total = (C.Crt_Price - C.Crt_Price * C.Crt_Discount) * C.Crt_Qty
    FROM Cart C
    INNER JOIN inserted i
      ON i.Crt_Id = c.Crt_Id
  END
END