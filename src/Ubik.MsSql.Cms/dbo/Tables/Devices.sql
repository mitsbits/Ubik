﻿CREATE TABLE [dbo].[Devices] (
    [Id]           INT             NOT NULL,
    [FriendlyName] NVARCHAR (512)  NOT NULL,
    [Path]         NVARCHAR (1024) NOT NULL,
    [Flavor]       INT             CONSTRAINT [DF_Devices_Flavor] DEFAULT ((0)) NOT NULL
);





