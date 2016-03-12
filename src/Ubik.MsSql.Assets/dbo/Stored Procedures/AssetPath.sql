-- =============================================
-- Author:		mitsbits
-- Create date: 2014/06/29
-- Description:	Get the path for an asset
-- =============================================
CREATE PROCEDURE AssetPath 

	@assetId int, 
	@version int = NULL
AS
BEGIN

	SET NOCOUNT ON;

	DECLARE @v int;
	IF  @version = NULL
		BEGIN
			SET @v = (SELECT TOP 1 a.CurrentVersion FROM Assets a WHERE a.Id = @assetId);
		END
	ELSE
		BEGIN
			SET @v = @version;
		END

	SELECT TOP 1 s.file_stream.GetFileNamespacePath(1) AS FullPath 
		FROM AssetVersions v 
		INNER JOIN AssetStore s 
		ON v.StreamId = s.stream_id
	WHERE v.AssetId = @assetId AND v.Version = @v;

END