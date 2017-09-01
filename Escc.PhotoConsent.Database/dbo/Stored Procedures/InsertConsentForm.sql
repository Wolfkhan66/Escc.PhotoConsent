-- =============================================
-- Author:		Cai Lehwald
-- Create date: 22/08/2017
-- Description:	Insert Consent Form 
-- =============================================
CREATE PROCEDURE [dbo].[InsertConsentForm]
	@DateCreated nvarchar(50),
	@CreatedBy nvarchar(50),
	@ProjectName nvarchar(50),
	@DateSubmitted nvarchar(50),
	@ConsentGiven bit,
	@Notes nvarchar(max),
	@GUID nvarchar(max),
	@Deleted bit,
	@PaymoNumber nvarchar(50)

AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [dbo].[ConsentForms] ([DateCreated], [CreatedBy], [ProjectName], [DateSubmitted], [ConsentGiven], [Notes], [GUID], [Deleted], [PaymoNumber]) 
	VALUES (@DateCreated, @CreatedBy, @ProjectName, @DateSubmitted, @ConsentGiven, @Notes, @GUID, @Deleted, @PaymoNumber)
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertConsentForm] TO [PhotoConsentUser]
    AS [dbo];

