CREATE TABLE [dbo].[Mimes] (
    [Id]           INT            NOT NULL,
    [Name]         NVARCHAR (50)  NOT NULL,
    [ContentType]  NVARCHAR (255) NOT NULL,
    [Extension]    VARCHAR (15)   NOT NULL,
    [DetailsTitle] NVARCHAR (255) NULL,
    [DetailsLink]  NVARCHAR (255) NULL
);

