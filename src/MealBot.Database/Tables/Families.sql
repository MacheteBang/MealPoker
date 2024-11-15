-- TODO: Configure unique constraint on Family.Code
CREATE TABLE [dbo].[Families]
(
   [FamilyId] UNIQUEIDENTIFIER NOT NULL,
   [Name] NVARCHAR(128) NOT NULL,
   [Description] NVARCHAR(512) NULL,
   [Code] VARCHAR(32) NOT NULL,

   CONSTRAINT [PK_Families] PRIMARY KEY ([FamilyId]),
   CONSTRAINT [UQ_Families_Code] UNIQUE ([Code])
)