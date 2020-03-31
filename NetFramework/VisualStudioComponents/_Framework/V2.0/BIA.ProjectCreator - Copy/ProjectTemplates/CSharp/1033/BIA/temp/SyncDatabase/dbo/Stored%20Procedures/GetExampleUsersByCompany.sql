CREATE PROCEDURE [dbo].[GetExampleUsersByCompany]
	@Company nvarchar(50) = NULL,
	@ExternalCompany nvarchar(50) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT *
	FROM [dbo].[User] AS A
	WHERE
	A.Company = @Company OR
	A.ExternalCompany = @ExternalCompany

END