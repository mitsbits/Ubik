-- =============================================
-- Author:		mitsbits
-- Create date: 2014/06/11
-- Description:	Gets the root path of a specified by name file tabe
-- =============================================
CREATE PROCEDURE [dbo].[FileTableRootPath] 
	@tableName nvarchar(50)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @path nvarchar(MAX);
	SET @path =(SELECT TOP 1 FileTableRootPath(@tableName));
	SELECT @path;
END