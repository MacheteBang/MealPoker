CREATE TABLE [dbo].[UserMealRatings]
(
   [MealRatingId] INT IDENTITY(1, 1),
   [UserId] UNIQUEIDENTIFIER NOT NULL,
   [MealId] UNIQUEIDENTIFIER NOT NULL,
   [Rating] VARCHAR(16) NOT NULL,

   CONSTRAINT [PK_UserMealRatings] PRIMARY KEY ([UserId], [MealId]),
   CONSTRAINT [FK_UserMealRatings_Meals] FOREIGN KEY ([MealId]) REFERENCES [dbo].[Meals] ([MealId]) ON DELETE CASCADE,
   CONSTRAINT [FK_UserMealRatings_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([UserId])
)