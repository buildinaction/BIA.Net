/*
 Pre-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be executed before the build script.	
 Use SQLCMD syntax to include a file in the pre-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the pre-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
DECLARE @SQLString NVARCHAR(500);
DECLARE @ParmDefinition nvarchar(500);
DECLARE @ver nvarchar(255);

SET @SQLString = N'SELECT  @verOUT=[Value] FROM [Version] WHERE [Name] = ''AppCode'';';
SET @ParmDefinition = N'@verOUT varchar(30) OUTPUT';

BEGIN TRY 
EXECUTE sp_executesql @SQLString,  @ParmDefinition, @verOUT=@ver OUTPUT;
SELECT @ver;
END TRY
BEGIN CATCH
	SET @ver = NULL
END CATCH


--IF @ver IS NOT NULL And @ver<'1.0.1'
--BEGIN
--SET @SQLString =
--     N'Select mov.Id, mem.User_Id into #TEMP from  [dbo].[Movements] mov, [dbo].[Member] mem where mem.Id = mov.Member_Id; ' +
--	'ALTER TABLE [dbo].[Movements] ADD [User_Id] INT; ' +
--	'UPDATE mov ' +
--	'SET mov.[User_Id] = temp.User_Id ' +
--	'FROM [dbo].[Movements] AS mov ' +
--	'INNER JOIN #TEMP as temp ON mov.Id = temp.Id ';
--	EXECUTE sp_executesql @SQLString;
--END