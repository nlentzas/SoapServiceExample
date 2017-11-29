CREATE PROCEDURE [dbo].[user_Login]
	@UserName NVARCHAR(50),  @Pass NVARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRY
		IF EXISTS (SELECT  Username,[Password]
				   FROM Logins
				   WHERE UserName=@UserName AND [Password]=(SELECT SUBSTRING(master.dbo.fn_varbintohexstr(HASHBYTES('SHA1',@pass)), 3, 32)))
			SELECT 1
		ELSE 
			SELECT 0
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
end