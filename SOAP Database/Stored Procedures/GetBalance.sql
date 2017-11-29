CREATE PROCEDURE [dbo].[GetBalance]
	-- Add the parameters for the stored procedure here
	@Username nvarchar(50), @UserBalance int OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	BEGIN TRY
		SELECT @UserBalance = Balance FROM Balances as b
					   INNER JOIN Logins as l ON b.ID = l.ID 
					   WHERE l.Username = @Username
							
	END TRY
	BEGIN CATCH
		SELECT  
		 ERROR_NUMBER() AS ErrorNumber  
		,ERROR_SEVERITY() AS ErrorSeverity  
		,ERROR_STATE() AS ErrorState  
		,ERROR_PROCEDURE() AS ErrorProcedure  
		,ERROR_LINE() AS ErrorLine  
		,ERROR_MESSAGE() AS ErrorMessage; 
	END CATCH
END