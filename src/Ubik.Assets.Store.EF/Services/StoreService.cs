using Mehdime.Entity;
using System;
using System.IO;
using System.Threading.Tasks;
using Ubik.Assets.Store.Core;
using Ubik.Assets.Store.Core.Contracts;
using Ubik.Assets.Store.EF.Contracts;
using Ubik.Assets.Store.EF.POCO;
using Ubik.EF6.Contracts;

namespace Ubik.Assets.Store.EF.Services
{
    public class StoreService : IStoreService<Guid>, IAssetService<int>
    {
        private readonly IDbContextScopeFactory _dbContextScopeFactory;
        private readonly IAssetStoreProjectionRepository _storeRepo;
        private readonly IMimeRepository _mimeRepo;
        private readonly IConflictingNamesResolver _nameResolver;
        private readonly IAssetDirectoryStrategy<int> _direcotryStartegy;
        private readonly IAssetRepository _assetRepo;

        public StoreService(IDbContextScopeFactory dbContextScopeFactory, IAssetStoreProjectionRepository storeRepo, IMimeRepository mimeRepo, IConflictingNamesResolver nameResolver, IAssetDirectoryStrategy<int> direcotryStartegy, IAssetRepository assetRepo)
        {
            _dbContextScopeFactory = dbContextScopeFactory;
            _storeRepo = storeRepo;
            _mimeRepo = mimeRepo;
            _nameResolver = nameResolver;
            _direcotryStartegy = direcotryStartegy;
            _assetRepo = assetRepo;
        }

        public async Task<IFileInfo<Guid>> Upload(byte[] content, string fileName, string parentFolder = default(string))
        {

            using (var db = _dbContextScopeFactory.Create())
            {
                var exists = await _storeRepo.Exists(fileName, parentFolder);
                if (exists) fileName = await _nameResolver.Resolve(fileName);

                var id = await _storeRepo.Add(fileName, content, parentFolder);

                var projection = await _storeRepo.GetAsync(x => x.stream_id == id);
                var result = new FileItemInfo<Guid>()
                {
                    CreationDate = projection.creation_time.DateTime,
                    FileSize = projection.cached_file_size ?? 0,
                    FileType = projection.file_type,
                    FullPath = projection.full_path,
                    IsDirectory = projection.is_directory,
                    LastRead = (projection.last_access_time.HasValue) ? projection.last_access_time.Value.DateTime : default(DateTime?),
                    LastWrite = projection.last_write_time.DateTime,
                    Name = projection.name,
                    Id = projection.stream_id
                };
                var ext = Path.GetExtension(fileName);
                var mime = await _mimeRepo.GetAsync(x => x.Extension == ext);
                if (mime != null) result.MimeType = mime.ContentType;
                return result;
            }
        }

        public async Task<IAssetInfo<int>> Create(string name, AssetState state, byte[] content, string fileName)
        {
            using (var db = _dbContextScopeFactory.Create())
            {
                var asset = new AssetItemInfo<int>();
                var sepProvider = _assetRepo as ISequenceRepository<Asset>;
                asset.Id = await sepProvider.GetNext();
                asset.Name = name;
                asset.State = state;
                var versionInfo = new VersionItemInfo() { Version = 1 };
                var parentDirecotry = _direcotryStartegy.ParentFolder(asset);
                var fileInfo = await Upload(content, fileName, parentDirecotry);
                var streamId = fileInfo.Id;
                versionInfo.FileInfo = fileInfo;
                asset.CurrentFile = versionInfo;

                var entity = new Asset()
                {
                    Id = asset.Id,
                    Name = asset.Name,
                    State = (int)asset.State,
                    CurrentVersion = 1,
                };
                entity.Versions.Add(new AssetVersion() { AssetId = asset.Id, StreamId = streamId, Version = 1 });
                await _assetRepo.CreateAsync(entity);
                await db.SaveChangesAsync();
                return asset;
            }
        }
    }
}