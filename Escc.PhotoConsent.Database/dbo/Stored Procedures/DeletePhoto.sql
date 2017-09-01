
-- =============================================
-- Author:		Cai Lehwald
-- Create date: 22/08/2017
-- Description:	Insert Photo
-- =============================================
CREATE PROCEDURE [dbo].[DeletePhoto]
	@ParticipantID int
AS
BEGIN
	SET NOCOUNT ON;

DELETE FROM Photos WHERE [ParticipantID] = @ParticipantID  
END


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeletePhoto] TO [PhotoConsentUser]
    AS [dbo];

