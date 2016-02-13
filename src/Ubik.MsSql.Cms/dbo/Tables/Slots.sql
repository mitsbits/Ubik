CREATE TABLE [dbo].[Slots] (
    [SectionId]  INT        NOT NULL,
    [Enabled]    BIT        NOT NULL,
    [Ordinal]    INT        NOT NULL,
    [ModuleType] NCHAR (50) NOT NULL,
    [ModuleInfo] XML        NULL
);



