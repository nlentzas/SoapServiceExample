CREATE PROCEDURE [dbo].[CreateLogin]
	-- Add the parameters for the stored procedure here
	@username nvarchar(50), @password nvarchar(50), @name nvarchar(50),@lastname nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	BEGIN TRANSACTION
	BEGIN TRY

		INSERT INTO Logins (ID,Username,[Password])
		VALUES ((SELECT ID FROM Clients WHERE Name=@name AND Last_Name=@lastname),@username, (SELECT SUBSTRING(master.dbo.fn_varbintohexstr(HASHBYTES('SHA1',@password)), 3, 32)))


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