-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[UpdateCommissioningOfficer]
	@Name nvarchar(50),
	@Email nvarchar(50),
	@ContactNumber nvarchar(50),
    @OfficerID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE CommissioningOfficer SET  
	[Name] = @Name, [Email] = @Email, [ContactNumber] = @ContactNumber
WHERE [OfficerID] = @OfficerID 
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateCommissioningOfficer] TO [PhotoConsentUser]
    AS [dbo];

