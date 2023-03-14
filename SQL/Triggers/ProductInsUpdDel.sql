CREATE OR ALTER TRIGGER ProductInsUpdDel
ON Product
AFTER INSERT,
      UPDATE,
      DELETE
AS
BEGIN
  IF EXISTS(SELECT * FROM inserted)
  BEGIN
    INSERT INTO OnStock (OnStk_Product) (SELECT Pr_Id 
                                         FROM inserted 
                                         WHERE Pr_Id NOT IN (SELECT OnStk_Product FROM OnStock)
                                         )
  END
END