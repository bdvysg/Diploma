CREATE OR ALTER PROCEDURE GetListOfCriticalItems
AS
BEGIN
  SELECT
        Pr_Id AS [��� ������],
        Pr_Title AS [�����],
        Br_Title AS [��������],
        Catg_Title AS [��������],
        Pr_Price AS [ֳ�� ���.],
        Pr_Reorder AS [��������� �������],
        OnStk_Quantity AS [� ��������]
  FROM Product
  INNER JOIN OnStock
    ON Pr_Id = OnStk_Product
  INNER JOIN Brand
    ON Pr_Brand = Br_Id
  INNER JOIN Category
    ON Pr_Category = Catg_Id
  WHERE OnStk_Quantity < Pr_Reorder
END