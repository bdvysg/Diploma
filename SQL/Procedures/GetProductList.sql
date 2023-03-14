CREATE OR ALTER PROCEDURE GetProductsList  -- Процедура выводит список продуктов для формы Product
  @SearchText varchar(200),
  @StartIndex int,
  @RowsCount int
  AS
BEGIN
  SELECT p.Pr_Id, 
         p.Pr_Title, 
         p.Pr_Qty, 
         b.Br_Title, 
         p.Pr_BarCode, 
         c.Catg_Title, 
         p.Pr_PriceOpt,
         p.Pr_Price, 
         p.Pr_Reorder,
         OnStk_Quantity
  FROM Product AS p 
  INNER JOIN Brand AS b 
    ON b.Br_Id = p.Pr_Brand 
  INNER JOIN Category AS c 
    ON c.Catg_Id = p.Pr_Category 
  INNER JOIN OnStock 
    ON OnStk_Product = p.Pr_Id
  WHERE CONCAT(b.Br_Title, c.Catg_Title, p.Pr_Title) LIKE '%' + @SearchText + '%'
  ORDER BY p.Pr_Id
  OFFSET @StartIndex ROWS
  FETCH NEXT @RowsCount ROWS ONLY;
END