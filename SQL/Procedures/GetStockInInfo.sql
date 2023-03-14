CREATE OR ALTER PROCEDURE GetStockInInfo
  @id int
AS
BEGIN
  SELECT 
    Sti_Id AS [Номер замовлення],
    Sti_StockInBy AS [Замовник],
    Sti_Date AS [Дата],
    Sup_Name AS [Постачальник],
    Sis_Title AS [Статус],
    CASE Sti_IsConfirmed 
      WHEN 0 THEN 'Не підтверджено'
      WHEN 1 THEN 'Підтверджено'
    END AS [Підтвердження],
    Pr_Id AS [Код товару],
    Pr_Title AS [Назва товару],
    Sip_Quantity AS [Кількість],
    Pr_PriceOpt AS [Ціна(грн.)],
    (SELECT SUM(Sip_Quantity * Pr_PriceOpt)
    FROM StockInProduct
    INNER JOIN Product ON Pr_Id = Sip_Product
    WHERE Sip_Doc = @id) AS [Всього]
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