-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[DeleteParticipant]
	@ParticipantID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
DELETE FROM Participants WHERE [ParticipantID] = @ParticipantID  
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteParticipant] TO [PhotoConsentUser]
    AS [dbo];

