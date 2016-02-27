CREATE TABLE [dbo].[Sections] (
    [Id]           INT             NOT NULL,
    [DeviceId]     INT             NOT NULL,
    [Identifier]   VARCHAR (512)   NOT NULL,
    [FriendlyName] NVARCHAR (1024) NOT NULL,
    [ForFlavor]    INT             NOT NULL
);



