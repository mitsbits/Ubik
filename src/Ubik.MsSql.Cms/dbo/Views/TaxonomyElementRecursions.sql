CREATE VIEW dbo.TaxonomyElementRecursions
AS
WITH cteRecurs(AncestorId, Id, ParentId, Depth) AS (SELECT        Id AS AncestorId, Id, ParentId, Depth
                                                                                                                FROM            dbo.TaxonomyElements
                                                                                                                UNION ALL
                                                                                                                SELECT        cteRecurs_2.AncestorId, TaxonomyElements_1.Id, TaxonomyElements_1.ParentId, TaxonomyElements_1.Depth
                                                                                                                FROM            cteRecurs AS cteRecurs_2 INNER JOIN
                                                                                                                                         dbo.TaxonomyElements AS TaxonomyElements_1 ON cteRecurs_2.Id = TaxonomyElements_1.ParentId)
    SELECT        TOP (100) PERCENT AncestorId, ParentId, Id, Depth
     FROM            cteRecurs AS cte
     ORDER BY AncestorId, ParentId, Id
GO


