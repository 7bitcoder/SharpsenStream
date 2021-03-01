/* Procedures Related to Stream*/
USE SharpsenStream

/* ==================================================================== */
GO
CREATE PROCEDURE GetStream @StreamName varchar(256)
AS
BEGIN
	SELECT StreamId, OwnerId, StreamName, Live, Title, Description, ChatId, StartTime FROM Stream
		WHERE Stream.StreamName = @StreamName;
END

GO
/* ==================================================================== */
CREATE PROCEDURE GetChatRooms
AS
BEGIN
	SELECT ChatId FROM Stream;
END
/* ==================================================================== */
GO
CREATE PROCEDURE AuthenticateStreamInit @StreamName varchar(256), @Token varchar(512)
AS
IF EXISTS (SELECT Token FROM Stream WHERE StreamName = @StreamName AND Token = @Token)
BEGIN
   RETURN 1 
END
ELSE
BEGIN
    RETURN 0
END
/* ==================================================================== */
GO
CREATE PROCEDURE SetStreamStartTime @StreamName varchar(256)
AS
BEGIN
UPDATE Stream SET StartTime=GETDATE() WHERE StreamName = @StreamName
END
/* ==================================================================== */
GO
CREATE PROCEDURE EndStream @StreamName varchar(256)
AS
BEGIN
INSERT INTO StreamsHistory (StreamId, OwnerId, StreamName, Title, Description, Token, StartTime,	EndTime)
SELECT StreamId, OwnerId, StreamName, Title, Description, Token, StartTime, GETDATE() FROM Stream WHERE StreamName = @StreamName
END
/* ==================================================================== */
