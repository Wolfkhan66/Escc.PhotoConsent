-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[UpdateParticipant]
	@Name nvarchar(50),
	@Email nvarchar(50),
	@ContactNumber nvarchar(50),
    @ParticipantID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE Participants SET  
	[Name] = @Name, [Email] = @Email, [ContactNumber] = @ContactNumber
WHERE [ParticipantID] = @ParticipantID 
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateParticipant] TO [PhotoConsentUser]
    AS [dbo];

