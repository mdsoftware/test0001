SELECT
t.ProductId,
COUNT(1) AS ProductFirst
FROM (
SELECT c.CustomerId AS CustomerId,
(SELECT TOP 1 ProductId FROM Sales s WHERE s.CustomerId = c.CustomerId ORDER BY s.DateCreated) AS ProductId
FROM (SELECT CustomerId FROM Sales GROUP BY CustomerId) c
) t GROUP BY ProductId
