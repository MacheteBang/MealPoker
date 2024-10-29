CREATE TABLE [dbo].[MealParts]
(
   [MealPartId] INT NOT NULL,
   [MealId] UNIQUEIDENTIFIER NOT NULL,
   [Category] VARCHAR(16) NOT NULL,
   [Name] NVARCHAR(128) NOT NULL,
   [Description] NVARCHAR(512) NULL,
   [Url] NVARCHAR(4000) NULL,

   CONSTRAINT [PK_MealParts] PRIMARY KEY ([MealPartId]),
   CONSTRAINT [FK_MealPartsMeals] FOREIGN KEY ([MealId]) REFERENCES [Meals] ([MealId])
)