
CREATE VIEW [dbo].[AssetStoreProjections]
AS
SELECT        stream_id, file_stream.GetFileNamespacePath(1) AS full_path, name, file_type, creation_time, last_write_time, last_access_time, is_directory, 
                         cached_file_size
FROM            dbo.AssetStore