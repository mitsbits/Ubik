-- =============================================
-- Author:		mitsbitrs
-- Create date: 2014/06/01
-- Description:	Add a file to Asset Store and return it's Id
-- =============================================
CREATE PROCEDURE AssetStoreAdd
	@parent nvarchar(50) = NULL, 
	@filename nvarchar(255),
	@filedata varbinary(max),
	@stream_id uniqueidentifier out
AS
BEGIN

	SET NOCOUNT ON;
	DECLARE @docid uniqueidentifier;
	DECLARE @parentid uniqueidentifier;
	
	
	IF @parent != NULL AND LEN(@parent) > 0
		BEGIN
			SET @parentid = (SELECT TOP 1 stream_id FROM AssetStore WHERE name = @parent AND is_directory = 1);
		
			IF @parentid = NULL
				BEGIN
					SET @parentid = NEWID();
					INSERT INTO AssetStore (stream_id, name, is_directory)
					VALUES (@parentid, @parent, 1);
				END
	
			DECLARE @path        HIERARCHYID
			DECLARE @new_path    VARCHAR(675)
	
			SELECT @path = path_locator
				FROM AssetStore
				WHERE stream_id = @parentid 

			SELECT @new_path = @path.ToString()     +
				CONVERT(VARCHAR(20), CONVERT(BIGINT, SUBSTRING(CONVERT(BINARY(16), NEWID()), 1, 6))) + '.' +
				CONVERT(VARCHAR(20), CONVERT(BIGINT, SUBSTRING(CONVERT(BINARY(16), NEWID()), 7, 6))) + '.' +
				CONVERT(VARCHAR(20), CONVERT(BIGINT, SUBSTRING(CONVERT(BINARY(16), NEWID()), 13, 4))) + '/'	
			
			SET @docid  = NEWID();
			INSERT INTO AssetStore(stream_id, name, file_stream, path_locator)
			VALUES(@docid, @filename, @filedata, @new_path )
		END
	ELSE
		BEGIN
			SET @docid  = NEWID();
			INSERT INTO AssetStore(stream_id, name, file_stream)
			VALUES(@docid, @filename, @filedata)
		END

	SET @stream_id = @docid;
END