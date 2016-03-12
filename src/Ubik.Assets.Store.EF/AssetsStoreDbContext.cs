using System;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Ubik.Assets.Store.EF.POCO;
using Ubik.EF6;

namespace Ubik.Assets.Store.EF
{
    public class AssetsStoreDbContext : SequenceProviderDbContext
    {

        private const string AssetStoreTableName = "AssetStore";



        #region Props
        public DbSet<Asset> Assets { get; set; }
        public DbSet<AssetVersion> Versions { get; set; }

        public DbSet<Mime> Mimes { get; set; }

        public DbSet<AssetStoreProjection> Projections { get; set; }
        #endregion

        public AssetsStoreDbContext(string connectionstring) : base(connectionstring)
        {
        }


        #region Stored Procedures
        public virtual async Task<Guid> AssetStoreAdd(string parent, string filename, byte[] filedata)
        {


            if (Database.Connection.State == ConnectionState.Closed) await Database.Connection.OpenAsync();
            using (var cmd = Database.Connection.CreateCommand())
            {

                cmd.CommandText = "[AssetStoreAdd]";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("parent", GetParentDirectories(parent)));
                cmd.Parameters.Add(new SqlParameter("filename", filename));
                cmd.Parameters.Add(new SqlParameter("filedata", filedata));

                var idParam = new SqlParameter("stream_id", SqlDbType.UniqueIdentifier) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(idParam);

                await cmd.ExecuteNonQueryAsync();

                return Guid.Parse(idParam.Value.ToString());
            }
        }

        private static DataTable GetParentDirectories(string parent)
        {
            var result = new DataTable();
            result.Columns.Add("Item", typeof(string));
            result.Columns.Add("SortOrder", typeof(int));

            if (string.IsNullOrWhiteSpace(parent)) return result;
            var counter = 1;
            foreach (var item in parent.Split(new[] { '/', '.', '\\' }))
            {
                result.Rows.Add(new object[] { item, counter });
                counter++;
            }
            return result;
        }

        public virtual ObjectResult<string> FileTableRootPath(string tableName)
        {
            var tableNameParameter = tableName != null ?
                new ObjectParameter("tableName", tableName) :
                new ObjectParameter("tableName", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("FileTableRootPath", tableNameParameter);
        }

        public virtual int StreamIdFromPath(string path, ObjectParameter id)
        {
            var pathParameter = path != null ?
                new ObjectParameter("path", path) :
                new ObjectParameter("path", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("StreamIdFromPath", pathParameter, id);
        }

        public virtual ObjectResult<string> AssetPath(Nullable<int> assetId, Nullable<int> version)
        {
            var assetIdParameter = assetId.HasValue ?
                new ObjectParameter("assetId", assetId) :
                new ObjectParameter("assetId", typeof(int));

            var versionParameter = version.HasValue ?
                new ObjectParameter("version", version) :
                new ObjectParameter("version", typeof(int));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("AssetPath", assetIdParameter, versionParameter);
        }
        #endregion

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Configurations.Add(new AssetConfig());
            modelBuilder.Configurations.Add(new AssetVersionConfig());
            modelBuilder.Configurations.Add(new MimeConfig());
            modelBuilder.Configurations.Add(new AssetStoreProjectionConfig());
        }
    }

    #region Entity Configuration
    internal class AssetConfig : EntityTypeConfiguration<Asset>
    {
        public AssetConfig()
        {
            ToTable("Assets");
            HasKey(x => x.Id);
            HasMany(x => x.Versions)
                .WithRequired(x => x.Asset)
                .HasForeignKey(x => x.AssetId);
        }
    }

    internal class AssetVersionConfig : EntityTypeConfiguration<AssetVersion>
    {
        public AssetVersionConfig()
        {
            ToTable("AssetVersions");
            HasKey(x => new { x.AssetId, x.Version });
            HasRequired(x => x.Asset)
                .WithMany(a => a.Versions)
                .HasForeignKey(a => a.AssetId);


        }
    }

    internal class MimeConfig : EntityTypeConfiguration<Mime>
    {
        public MimeConfig()
        {
            ToTable("Mimes");
            HasKey(x => x.Id);
            HasMany(x => x.Assets)
                .WithRequired(x => x.Mime)
                .HasForeignKey(x => x.MimeId);
        }
    }

    internal class AssetStoreProjectionConfig : EntityTypeConfiguration<AssetStoreProjection>
    {
        public AssetStoreProjectionConfig()
        {
            ToTable("AssetStoreProjections");
            HasKey(x => x.stream_id);
        }
    }
    #endregion
}