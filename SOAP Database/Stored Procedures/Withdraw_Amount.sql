CREATE PROCEDURE [dbo].[Withdraw_Amount]
	-- Add the parameters for the stored procedure here
	@Amount int, @Username nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	BEGIN TRANSACTION
		BEGIN TRY
			DECLARE @User_id int
			SET @User_id = (SELECT ID FROM Logins WHERE Username=@Username)
			IF (((SELECT Balance FROM Balances WHERE ID = @User_id) - @Amount) > 0)
				UPDATE Balances SET Balance = Balance - @Amount WHERE ID = @User_id
			INSERT INTO Transaction_Log ([User], Amount, [Transaction]) VALUES (@User_id, @Amount, (SELECT ID FROM Transactions WHERE Transaction_Type = 'Withdraw'))
		END TRY
		BEGIN CATCH
			SELECT  
			ERROR_NUMBER() AS ErrorNumber  
			,ERROR_SEVERITY() AS ErrorSeverity  
			,ERROR_STATE() AS ErrorState  
			,ERROR_PROCEDURE() AS ErrorProcedure  
			,ERROR_LINE() AS ErrorLine  
			,ERROR_MESSAGE() AS ErrorMessage;
			IF @@TRANCOUNT > 0  
				ROLLBACK TRANSACTION; 
		END CATCH

	IF @@TRANCOUNT > 0  
		COMMIT TRANSACTION;  
END

GO