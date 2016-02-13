CREATE TABLE [dbo].[UbikRoleClaims] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [RoleId]     INT            NOT NULL,
    [ClaimType]  NVARCHAR (MAX) NOT NULL,
    [CLaimValue] NVARCHAR (MAX) NOT NULL
);





