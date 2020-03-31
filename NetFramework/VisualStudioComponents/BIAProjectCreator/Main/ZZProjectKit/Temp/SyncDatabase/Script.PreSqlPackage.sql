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

IF object_id('IsLowerVersion', 'FN') IS NOT NULL
       DROP FUNCTION dbo.IsLowerVersion;
GO

CREATE FUNCTION IsLowerVersion (@v1 varchar(30), @v2 varchar(30))
       RETURNS bit
       BEGIN
             DECLARE @isUpper bit;

             declare @v1Major varchar(30)
             declare @v1Minor varchar(30)
             declare @v1Bug varchar(30)
             
             declare @v2Major varchar(30)
             declare @v2Minor varchar(30)
             declare @v2Bug varchar(30)

             SET @v1Major = LEFT(@v1, CHARINDEX('.', @v1)-1)
             SET @v1Minor = substring(@v1, CHARINDEX('.', @v1)+1, LEN(@v1))
             SET @v1Bug = substring(@v1Minor, CHARINDEX('.', @v1Minor)+1, LEN(@v1Minor))
             SET @v1Minor = LEFT(@v1Minor, CHARINDEX('.', @v1Minor)-1)
             
             SET @v2Major = LEFT(@v2, CHARINDEX('.', @v2)-1)
             SET @v2Minor = substring(@v2, CHARINDEX('.', @v2)+1, LEN(@v2))
             SET @v2Bug = substring(@v2Minor, CHARINDEX('.', @v2Minor)+1, LEN(@v2Minor))
             SET @v2Minor = LEFT(@v2Minor, CHARINDEX('.', @v2Minor)-1)
             
           SELECT @isUpper =
                    case 
                          when CONVERT(int, @v1Major) < CONVERT(int, @v2Major) then 1
                          when CONVERT(int, @v1Major) > CONVERT(int, @v2Major) then 0
                          when CONVERT(int, @v1Minor) < CONVERT(int, @v2Minor) then 1
                          when CONVERT(int, @v1Minor) > CONVERT(int, @v2Minor) then 0
                          when CONVERT(int, @v1Bug) < CONVERT(int, @v2Bug) then 1
                          when CONVERT(int, @v1Bug) > CONVERT(int, @v2Bug) then 0
                          else 0
                    END
             RETURN @isUpper;
       END
GO



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


--IF @ver IS NOT NULL And dbo.IsLowerVersion(@ver,'1.0.0') = 1
--BEGIN
--SET @SQLString =
--	 N'Insert Into dbo.Site (Title) VALUES (''test version older than 1.0.0'')';

--	EXECUTE sp_executesql @SQLString;
--END