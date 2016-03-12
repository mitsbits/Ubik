using Mehdime.Entity;
using System;
using System.IO;
using System.Threading.Tasks;
using Ubik.Assets.Store.Core;
using Ubik.Assets.Store.Core.Contracts;
using Ubik.Assets.Store.EF.Contracts;

namespace Ubik.Assets.Store.EF.Services
{
    public class StoreService : IStoreService<Guid>
    {
        private readonly IDbContextScopeFactory _dbContextScopeFactory;
        private readonly IAssetStoreProjectionRepository _storeRepo;
        private readonly IMimeRepository _mimeRepo;
        private readonly IConflictingNamesResolver _nameResolver;

        public StoreService(IDbContextScopeFactory dbContextScopeFactory, IAssetStoreProjectionRepository storeRepo, IMimeRepository mimeRepo, IConflictingNamesResolver nameResolver)
        {
            _dbContextScopeFactory = dbContextScopeFactory;
            _storeRepo = storeRepo;
            _mimeRepo = mimeRepo;
            _nameResolver = nameResolver;
        }

        public async Task<AssetInfo<Guid>> Upload(byte[] content, string fileName, string parentFolder = default(string))
        {

            using (var db = _dbContextScopeFactory.Create())
            {
                var exists = await _storeRepo.Exists(fileName, parentFolder);
                if (exists) fileName = await _nameResolver.Resolve(fileName);


                var id = await _storeRepo.Add(fileName, content, parentFolder);

                var projection = await _storeRepo.GetAsync(x => x.stream_id == id);
                var result = new AssetInfo<Guid>()
                {
                    CreationDate = projection.creation_time.DateTime,
                    FileSize = projection.cached_file_size ?? 0,
                    FileType = projection.file_type,
                    FullPath = projection.full_path,
                    IsDirectory = projection.is_directory,
                    LastRead = (projection.last_access_time.HasValue) ? projection.last_access_time.Value.DateTime : default(DateTime?),
                    LastWrite = projection.last_write_time.DateTime,
                    Name = projection.name,
                    StreamId = projection.stream_id
                };
                var ext = Path.GetExtension(fileName);
                var mime = await _mimeRepo.GetAsync(x => x.Extension == ext);
                if (mime != null) result.MimeType = mime.ContentType;
                return result;
            }
        }
    }
}