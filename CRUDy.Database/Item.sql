﻿CREATE TABLE [dbo].[Item]
(
	[Id] INT IDENTITY NOT NULL,
	[Title] NVARCHAR (75) NOT NULL,
	[Description] NVARCHAR (500) NOT NULL,
	CONSTRAINT PK_Item PRIMARY KEY (Id)
)
