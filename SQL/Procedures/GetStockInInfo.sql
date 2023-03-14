CREATE OR ALTER PROCEDURE GetStockInInfo
  @id int
AS
BEGIN
  SELECT 
    Sti_Id AS [����� ����������],
    Sti_StockInBy AS [��������],
    Sti_Date AS [����],
    Sup_Name AS [������������],
    Sis_Title AS [������],
    CASE Sti_IsConfirmed 
      WHEN 0 THEN '�� �����������'
      WHEN 1 THEN 'ϳ����������'
    END AS [ϳ�����������],
    Pr_Id AS [��� ������],
    Pr_Title AS [����� ������],
    Sip_Quantity AS [ʳ������],
    Pr_PriceOpt AS [ֳ��(���.)],
    (SELECT SUM(Sip_Quantity * Pr_PriceOpt)
    FROM StockInProduct
    INNER JOIN Product ON Pr_Id = Sip_Product
    WHERE Sip_Doc = @id) AS [������]
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