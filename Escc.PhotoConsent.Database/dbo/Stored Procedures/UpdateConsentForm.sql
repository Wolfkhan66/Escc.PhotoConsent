-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[UpdateConsentForm]
	@CreatedBy nvarchar(50),
	@ProjectName nvarchar(50),
	@Notes nvarchar(max),
    @FormID int,
	@ConsentGiven bit,
	@DateSubmitted nvarchar(50),
	@PaymoNumber nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE ConsentForms SET  
	[CreatedBy] = @CreatedBy, [ProjectName] = @ProjectName, [Notes] = @Notes, [ConsentGiven] = @ConsentGiven, [DateSubmitted] = @DateSubmitted, [PaymoNumber] = @PaymoNumber
WHERE [FormID] = @FormID 
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateConsentForm] TO [PhotoConsentUser]
    AS [dbo];

