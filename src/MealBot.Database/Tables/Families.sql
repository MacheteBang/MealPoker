CREATE TABLE [dbo].[Families]
(
   [FamilyId] UNIQUEIDENTIFIER NOT NULL,
   [Name] NVARCHAR(128) NOT NULL,
   [Description] NVARCHAR(512) NULL,
   [Code] VARCHAR(32) NOT NULL

   CONSTRAINT [PK_Families] PRIMARY KEY ([FamilyId])
)   