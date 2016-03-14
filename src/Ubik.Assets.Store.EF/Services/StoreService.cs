using Mehdime.Entity;
using System;
using System.IO;
using System.Threading.Tasks;
using Ubik.Assets.Store.Core;
using Ubik.Assets.Store.Core.Contracts;
using Ubik.Assets.Store.Core.Events;
using Ubik.Assets.Store.EF.Contracts;
using Ubik.Assets.Store.EF.POCO;
using Ubik.Domain.Core;
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
        private readonly IEventBus _eventBus;

        public StoreService(IDbContextScopeFactory dbContextScopeFactory, IAssetStoreProjectionRepository storeRepo, IMimeRepository mimeRepo, IConflictingNamesResolver nameResolver, IAssetDirectoryStrategy<int> direcotryStartegy, IAssetRepository assetRepo, IEventBus eventBus)
        {
            _dbContextScopeFactory = dbContextScopeFactory;
            _storeRepo = storeRepo;
            _mimeRepo = mimeRepo;
            _nameResolver = nameResolver;
            _direcotryStartegy = direcotryStartegy;
            _assetRepo = assetRepo;
            _eventBus = eventBus;
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
                await _eventBus.Publish(new FileUploadedEvent(result));
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
                var ext = Path.GetExtension(fileName);
                var mime = await _mimeRepo.GetAsync(x => x.Extension == ext);

                entity.Versions.Add(new AssetVersion() { AssetId = asset.Id, StreamId = streamId, Version = 1, MimeId = (mime != null) ? mime.Id : default(int?) });
                await _assetRepo.CreateAsync(entity);
                await db.SaveChangesAsync();
                await _eventBus.Publish(new AssetCreatedEvent(asset));
                return asset;
            }
        }

        public async Task Suspend(int id)
        {
            using (var db = _dbContextScopeFactory.Create())
            {
                var entity = await _assetRepo.GetAsync(x => x.Id == id);
                if (entity != null && entity.State != (int)AssetState.Suspended)
                {
                    entity.State = (int)AssetState.Suspended;
                    await db.SaveChangesAsync();
                    await _eventBus.Publish(new AssetStateChangeEvent<int>(id, AssetState.Suspended));
                }
            }
        }

        public async Task Acivate(int id)
        {
            using (var db = _dbContextScopeFactory.Create())
            {
                var entity = await _assetRepo.GetAsync(x => x.Id == id);
                if (entity != null && entity.State != (int)AssetState.Active)
                {
                    entity.State = (int)AssetState.Suspended;
                    await db.SaveChangesAsync();
                    await _eventBus.Publish(new AssetStateChangeEvent<int>(id, AssetState.Active));
                }
            }
        }

        public async Task<IAssetInfo<int>> AddNewVersion(int id, byte[] content, string fileName)
        {
            using (var db = _dbContextScopeFactory.Create())
            {
                var entity = await _assetRepo.GetAsync(x => x.Id == id);
                if (entity != null)
                {
                    var asset = new AssetItemInfo<int>() { Id = id, Name = entity.Name, State = (AssetState)entity.State };
                    var versionInfo = new VersionItemInfo() { Version = entity.CurrentVersion + 1 };
                    var parentDirecotry = _direcotryStartegy.ParentFolder(asset);
                    var fileInfo = await Upload(content, fileName, parentDirecotry);
                    var streamId = fileInfo.Id;
                    versionInfo.FileInfo = fileInfo;
                    asset.CurrentFile = versionInfo;
                    entity.CurrentVersion = versionInfo.Version;
                    var ext = Path.GetExtension(fileName);
                    var mime = await _mimeRepo.GetAsync(x => x.Extension == ext);
                    entity.Versions.Add(new AssetVersion() { AssetId = asset.Id, StreamId = streamId, Version = versionInfo.Version, MimeId = (mime != null) ? mime.Id : default(int?) });
                    await db.SaveChangesAsync();
                    return asset;
                }
                throw new ApplicationException(string.Format("Asset with id: {0} does not exist.", id));
            }
        }
    }
}