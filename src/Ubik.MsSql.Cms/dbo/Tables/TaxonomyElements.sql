CREATE TABLE [dbo].[TaxonomyElements] (
    [Id]         INT NOT NULL,
    [ParentId]   INT CONSTRAINT [DF_TaxonomyElements_ParentId] DEFAULT ((0)) NOT NULL,
    [DivisionId] INT NOT NULL,
    [Depth]      INT CONSTRAINT [DF_TaxonomyElements_Depth] DEFAULT ((1)) NOT NULL,
    [TextualId]  INT NOT NULL
);

