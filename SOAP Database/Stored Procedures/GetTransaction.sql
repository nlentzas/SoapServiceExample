CREATE PROCEDURE [dbo].[GetTransaction]
	-- Add the parameters for the stored procedure here
	@username nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT TOP(1) [Amount] as Amount, [Transaction_type] as [Transaction] FROM Transaction_Log tl
												INNER JOIN Transactions t on tl.[Transaction]=t.ID
												INNER JOIN Logins l on l.ID=tl.[User]
												WHERE l.Username=@username
												ORDER BY tl.ID DESC 
END