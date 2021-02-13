USE SharpsenStream

GO
CREATE PROCEDURE GetStream @StreamName varchar(256)
AS
BEGIN
	SELECT StreamId, OwnerId, StreamName, Live, Title, Description, ChatId FROM Stream
		WHERE Stream.StreamName = @StreamName;
END

GO
CREATE PROCEDURE GetChatRooms
AS
BEGIN
	SELECT ChatId FROM Stream;
END
