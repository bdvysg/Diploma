CREATE OR ALTER PROCEDURE GetListOfCriticalItems
AS
BEGIN
  SELECT
        Pr_Id AS [Код товару],
        Pr_Title AS [Назва],
        Br_Title AS [Виробник],
        Catg_Title AS [Категорія],
        Pr_Price AS [Ціна опт.],
        Pr_Reorder AS [Критичний залишок],
        OnStk_Quantity AS [В наявності]
  FROM Product
  INNER JOIN OnStock
    ON Pr_Id = OnStk_Product
  INNER JOIN Brand
    ON Pr_Brand = Br_Id
  INNER JOIN Category
    ON Pr_Category = Catg_Id
  WHERE OnStk_Quantity < Pr_Reorder
END