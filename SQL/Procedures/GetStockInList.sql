CREATE OR ALTER PROCEDURE GetStockInList
AS
BEGIN
  SELECT 
    Sti_Id,
    Sti_StockInBy,
    Sti_Date,
    Sup_Name,
    Sis_Title,
    CASE Sti_IsConfirmed 
      WHEN 0 THEN 'Не підтверджено'
      WHEN 1 THEN 'Підтверджено'
    END AS IsConfirmed
  FROM StockIn
  INNER JOIN StockInStatus
    ON Sti_Status = Sis_Id
  LEFT JOIN Supplier
    ON Sti_SupplierId = Sup_Id
END