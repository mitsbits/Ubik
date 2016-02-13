CREATE TABLE [dbo].[Textuals] (
    [Id]      INT             NOT NULL,
    [Subject] NVARCHAR (1024) NOT NULL,
    [Summary] VARBINARY (MAX) NULL,
    [Body]    VARBINARY (MAX) NULL
);

