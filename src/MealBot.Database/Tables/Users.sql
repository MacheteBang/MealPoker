CREATE TABLE [dbo].[Users]
(
   [UserId] UNIQUEIDENTIFIER NOT NULL,
   [EmailAddress] VARCHAR(256) NOT NULL,
   [AuthProvider] VARCHAR(32) NULL,
   [ExternalId] VARCHAR(256) NOT NULL,
   [FirstName] VARCHAR(128) NOT NULL,
   [LastName] VARCHAR(128) NOT NULL,
   [ProfileImageUrl] VARCHAR(4094) NULL,
   [RefreshToken] VARCHAR(128) NULL,
   [RefreshTokenExpiresAt] DATETIME2 NOT NULL,

   CONSTRAINT [PK_Users] PRIMARY KEY ([UserId])
)
