/* Procedures Related to Users */
USE SharpsenStream

/* ==================================================================== */
GO
CREATE PROCEDURE Login @Username varchar(256), @PasswordHash varchar(256)
AS
declare @Id int = NULL
SELECT @Id = (SELECT TOP 1 USerId FROM Users WHERE Username = @Username AND UserPassword = @PasswordHash)
IF (@Id is null)
BEGIN
	Select null
END
ELSE
BEGIN
   EXEC dbo.GetUser @Id
END
/* ==================================================================== */
GO
CREATE PROCEDURE GetUser @Id int
AS
BEGIN
   SELECT TOP 1 UserId, Username, Email, AvatarFilePath, Color FROM Users
		WHERE UserId = @Id
END
/* ==================================================================== */
GO
CREATE PROCEDURE SetToken @UserId int, @Token varchar(256), @Expire DateTime, @Refresh bit
AS
BEGIN

INSERT INTO UserTokens (OwnerId, Token, Expiration, Refresh)
	VALUES (@UserId, @Token, @Expire, @Refresh)
END

/* ==================================================================== */
GO
CREATE PROCEDURE DeleteToken @UserId int, @Token varchar(256)
AS
BEGIN
DELETE FROM UserTokens WHERE OwnerId = @UserId AND (Expiration < GETDATE() OR @Token = Token)
END
/* ==================================================================== */
