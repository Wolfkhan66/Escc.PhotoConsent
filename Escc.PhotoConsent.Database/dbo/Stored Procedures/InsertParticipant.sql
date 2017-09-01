-- =============================================
-- Author:		Cai Lehwald
-- Create date: 22/08/2017
-- Description:	Insert Participant
-- =============================================
CREATE PROCEDURE [dbo].[InsertParticipant]
	@FormID int,
	@Name nvarchar(50),
	@Email nvarchar(50),
	@ContactNumber nvarchar(50)

AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [dbo].[Participants]([FormID], [Name], [Email], [ContactNumber]) 
	VALUES (@FormID, @Name, @Email, @ContactNumber)
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertParticipant] TO [PhotoConsentUser]
    AS [dbo];

