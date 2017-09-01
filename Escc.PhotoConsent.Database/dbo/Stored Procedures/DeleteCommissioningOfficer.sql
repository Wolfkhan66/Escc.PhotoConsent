-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[DeleteCommissioningOfficer]
	@OfficerID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
DELETE FROM CommissioningOfficer WHERE [OfficerID] = @OfficerID  
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteCommissioningOfficer] TO [PhotoConsentUser]
    AS [dbo];

