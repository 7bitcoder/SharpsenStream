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
CREATE PROCEDURE GetNewToken @UserId int, @OldToken varchar(256) = NULL, @ExpireDays int = 365
AS
BEGIN

IF ( @OldToken IS NOT NULL)
BEGIN
	EXEC DeleteToken @UserId, @OldToken
END
/* ==================================================================== */
DECLARE @myid uniqueidentifier  
SET @myid = NEWID() 
INSERT INTO UserTokens (OwnerId, Token, Expiration)
 OUTPUT INSERTED .*
VALUES (@UserId,CONVERT(varchar(256), @myid), DATEADD(Day, @ExpireDays, GETDATE()))
END
/* ==================================================================== */
GO
CREATE PROCEDURE DeleteToken @UserId int, @Token varchar(256)
AS
BEGIN
DELETE FROM UserTokens WHERE OwnerId = @UserId AND (Expiration < GETDATE() OR @Token = Token)
END
/* ==================================================================== */
