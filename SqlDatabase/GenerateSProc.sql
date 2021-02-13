USE SharpsenStream

GO
CREATE PROCEDURE GetStream @StreamName varchar(256)
AS
BEGIN
	SELECT StreamId, OwnerId, StreamName, Live, Title, Description FROM Stream
		WHERE Stream.StreamName = @StreamName;
END
