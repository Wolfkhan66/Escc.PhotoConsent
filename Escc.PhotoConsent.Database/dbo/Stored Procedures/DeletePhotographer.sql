-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[DeletePhotographer]
	@PhotographerID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
DELETE FROM Photographers WHERE [PhotographerID] = @PhotographerID  
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeletePhotographer] TO [PhotoConsentUser]
    AS [dbo];

