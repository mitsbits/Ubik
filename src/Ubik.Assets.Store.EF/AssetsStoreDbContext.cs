using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Entity;
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

        private string _assetStoreRoot;

        #region Props

        public DbSet<Asset> Assets { get; set; }
        public DbSet<AssetVersion> Versions { get; set; }

        public DbSet<Mime> Mimes { get; set; }

        public DbSet<AssetStoreProjection> FileProjections { get; set; }

        public DbSet<AssetProjection> AssetProjections { get; set; }

        #endregion Props

        public string AssetStoreRoot
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_assetStoreRoot)) _assetStoreRoot = FileTableRootPath(AssetStoreTableName).Result;
                return _assetStoreRoot;
            }
        }

        public AssetsStoreDbContext(string connectionstring) : base(connectionstring)
        {
        }

        #region Stored Procedures

        public virtual async Task<Guid> AssetStoreAdd(string parent, string filename, byte[] filedata)
        {
            var shouldClose = false;

            if (Database.Connection.State == ConnectionState.Closed) { await Database.Connection.OpenAsync(); shouldClose = true; }
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
                if (shouldClose) cmd.Connection.Close();
                return Guid.Parse(idParam.Value.ToString());
            }
        }

        public static DataTable GetParentDirectories(string parent)
        {
            var result = new DataTable();
            result.Columns.Add("Item", typeof(string));
            result.Columns.Add("SortOrder", typeof(int));
            var delemeters = new[] { '/', '.', '\\' };
            parent = parent.TrimEnd(delemeters);
            if (string.IsNullOrWhiteSpace(parent)) return result;
            var counter = 1;
            foreach (var item in parent.Split(delemeters))
            {
                result.Rows.Add(new object[] { item, counter });
                counter++;
            }
            return result;
        }

        public virtual async Task<string> FileTableRootPath(string tableName)
        {
            var shouldClose = false;

            if (Database.Connection.State == ConnectionState.Closed) { await Database.Connection.OpenAsync(); shouldClose = true; }
            using (var cmd = Database.Connection.CreateCommand())
            {
                cmd.CommandText = "[FileTableRootPath]";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("tableName", tableName));

                var result = await cmd.ExecuteScalarAsync();
                if (shouldClose) cmd.Connection.Close();
                return result.ToString();
            }
        }

        public virtual async Task<Guid> StreamIdFromPath(string path)
        {
            var shouldClose = false;
            if (Database.Connection.State == ConnectionState.Closed) { await Database.Connection.OpenAsync(); shouldClose = true; }
            using (var cmd = Database.Connection.CreateCommand())
            {
                cmd.CommandText = "[StreamIdFromPath]";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("path", path));

                var idParam = new SqlParameter("stream_id", SqlDbType.UniqueIdentifier) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(idParam);

                await cmd.ExecuteNonQueryAsync();
                if (shouldClose) cmd.Connection.Close();
                return Guid.Parse(idParam.Value.ToString());
            }
        }

        public virtual async Task<string> AssetPath(int? assetId, int? version)
        {
            var shouldClose = false;
            if (Database.Connection.State == ConnectionState.Closed) { await Database.Connection.OpenAsync(); shouldClose = true; }
            using (var cmd = Database.Connection.CreateCommand())
            {
                cmd.CommandText = "[AssetPath]";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("assetId", assetId));
                cmd.Parameters.Add(new SqlParameter("version", version));

                var result = await cmd.ExecuteScalarAsync();
                if (shouldClose) cmd.Connection.Close();
                return result.ToString();
            }
        }

        #endregion Stored Procedures

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
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
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
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
        }
    }

    internal class AssetStoreProjectionConfig : EntityTypeConfiguration<AssetStoreProjection>
    {
        public AssetStoreProjectionConfig()
        {
            ToTable("AssetStoreProjections");
            HasKey(x => x.stream_id);
            Property(x => x.stream_id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
        }
    }

    internal class AssetProjectionConfig : EntityTypeConfiguration<AssetProjection>
    {
        public AssetProjectionConfig()
        {
            ToTable("AssetProjections");
            HasKey(x => x.Id);
            Property(x => x.stream_id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
        }
    }

    #endregion Entity Configuration
}