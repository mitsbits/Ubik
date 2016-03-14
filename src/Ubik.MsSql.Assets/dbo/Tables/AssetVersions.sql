CREATE TABLE [dbo].[AssetVersions] (
    [StreamId] UNIQUEIDENTIFIER NOT NULL,
    [AssetId]  INT              NOT NULL,
    [Version]  INT              NOT NULL,
    [MimeId]   INT              NULL
);



