-- =============================================
-- Author:		mitsbits
-- Create date: 2014/06/11
-- Description:	Get the stream Id for a given path
-- =============================================
CREATE PROCEDURE StreamIdFromPath 
	@path nvarchar(MAX), 
	@id uniqueidentifier OUT
AS
BEGIN

	SET NOCOUNT ON;

	DECLARE @locator hierarchyid;
	SET @locator = GetPathLocator(@path);
	SET @id = (SELECT TOP 1 a.stream_id FROM AssetStore a WHERE a.path_locator = @locator);
END