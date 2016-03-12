-- =============================================
-- Author:		mitsbitrs
-- Create date: 2014/06/01
-- Description:	Add a file to Asset Store and return it's Id
-- =============================================
CREATE PROCEDURE [dbo].[AssetStoreAdd]
	@parent OrderedStringListType READONLY, 
	@filename nvarchar(255),
	@filedata varbinary(max),
	@stream_id uniqueidentifier out
AS
BEGIN

	SET NOCOUNT ON;
	DECLARE @docid uniqueidentifier;
	DECLARE @parentid uniqueidentifier;
	
	
	IF (SELECT COUNT(1) FROM @parent) > 0
		BEGIN
		DECLARE @currentLevel int;
		SET @currentLevel = (SELECT MIN(SortOrder) FROM @parent) - 1;
		WHILE (1 = 1)
			BEGIN
			  SELECT @currentLevel = MIN(SortOrder)
			  FROM @parent WHERE SortOrder > @currentLevel;
			  IF @currentLevel IS NULL BREAK;
			  DECLARE @folder NVARCHAR(256);
			  SELECT @folder = Item FROM @parent WHERE SortOrder = @currentLevel;
			  IF @parentid IS NULL
				  BEGIN
				  SET @parentid = (SELECT TOP 1 stream_id FROM AssetStore WHERE name = @folder AND is_directory = 1 AND parent_path_locator = NULL);
				  IF @parentid IS NULL
					  BEGIN
						SET @parentid = NEWID();
						INSERT INTO AssetStore (stream_id, name, is_directory) VALUES (@parentid, @folder, 1);
					  END
				  END
			 ELSE
				  BEGIN
					DECLARE @path        HIERARCHYID
					DECLARE @new_path    VARCHAR(675)
	
					SELECT @path = path_locator
						FROM AssetStore
						WHERE stream_id = @parentid 

					SELECT @new_path = @path.ToString()     +
						CONVERT(VARCHAR(20), CONVERT(BIGINT, SUBSTRING(CONVERT(BINARY(16), NEWID()), 1, 6))) + '.' +
						CONVERT(VARCHAR(20), CONVERT(BIGINT, SUBSTRING(CONVERT(BINARY(16), NEWID()), 7, 6))) + '.' +
						CONVERT(VARCHAR(20), CONVERT(BIGINT, SUBSTRING(CONVERT(BINARY(16), NEWID()), 13, 4))) + '/';	
						
					SET @parentid = NEWID();

					INSERT INTO AssetStore(stream_id, name, path_locator, is_directory)
					VALUES(@parentid, @folder, @new_path, 1 );											
				  END
			END
	
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