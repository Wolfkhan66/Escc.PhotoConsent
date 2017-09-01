-- =============================================
-- Author:		Cai Lehwald
-- Create date: 22/08/2017
-- Description:	Insert Commissioning Officer
-- =============================================
CREATE PROCEDURE [dbo].[InsertCommissioningOfficer]
	@FormID int,
	@Name nvarchar(50),
	@Email nvarchar(50),
	@ContactNumber nvarchar(50)

AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [dbo].[CommissioningOfficer]([FormID], [Name], [Email], [ContactNumber]) 
	VALUES (@FormID, @Name, @Email, @ContactNumber)
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertCommissioningOfficer] TO [PhotoConsentUser]
    AS [dbo];

