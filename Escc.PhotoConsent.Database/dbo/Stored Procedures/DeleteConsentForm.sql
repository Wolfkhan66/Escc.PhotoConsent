-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[DeleteConsentForm]
	@FormID int,
	@Deleted bit
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE ConsentForms SET  
	 [Deleted] = @Deleted
WHERE [FormID] = @FormID 
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteConsentForm] TO [PhotoConsentUser]
    AS [dbo];

