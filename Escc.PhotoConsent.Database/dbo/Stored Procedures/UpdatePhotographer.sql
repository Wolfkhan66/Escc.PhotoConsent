-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[UpdatePhotographer]
	@Name nvarchar(50),
	@Email nvarchar(50),
	@ContactNumber nvarchar(50),
    @PhotographerID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE Photographers SET  
	[Name] = @Name, [Email] = @Email, [ContactNumber] = @ContactNumber
WHERE [PhotographerID] = @PhotographerID 
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdatePhotographer] TO [PhotoConsentUser]
    AS [dbo];

