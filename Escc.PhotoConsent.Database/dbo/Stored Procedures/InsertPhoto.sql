-- =============================================
-- Author:		Cai Lehwald
-- Create date: 22/08/2017
-- Description:	Insert Photo
-- =============================================
CREATE PROCEDURE [dbo].[InsertPhoto]
	@ParticipantID int,
	@Image image

AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [dbo].[Photos]( [ParticipantID], [Image]) 
	VALUES (@ParticipantID, @Image)
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertPhoto] TO [PhotoConsentUser]
    AS [dbo];

