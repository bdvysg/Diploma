CREATE OR ALTER PROCEDURE GetStockInInfo
  @id int
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
    END AS IsConfirmed,
    Pr_Id,
    Pr_Title,
    Sip_Quantity
  FROM StockIn
  INNER JOIN StockInStatus
    ON Sti_Status = Sis_Id
  LEFT JOIN Supplier
    ON Sti_SupplierId = Sup_Id
  LEFT JOIN StockInProduct
    ON Sti_Id = Sip_Doc
  LEFT JOIN Product
    ON Sip_Product = Pr_Id
  LEFT JOIN Category
    ON Pr_Category = Catg_Id 
  WHERE Sti_Id = @id
END