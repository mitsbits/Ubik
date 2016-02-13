using Mehdime.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Ubik.EF6;
using Ubik.Infra.Contracts;
using Ubik.Infra.DataManagement;

namespace Ubik.Web.EF
{
    public class ElmahDbContext : DbContext
    {
        public ElmahDbContext()
            : base("cmsconnectionstring")
        { }

        public ElmahDbContext(string connString)
            : base(connString)
        {
        }

        public DbSet<PersistedExceptionLog> Logs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new PersistedExceptionLogConfig());
            base.OnModelCreating(modelBuilder);
        }

        private class PersistedExceptionLogConfig : EntityTypeConfiguration<PersistedExceptionLog>
        {
            public PersistedExceptionLogConfig()
            {
                ToTable("ELMAH_Error").
                    HasKey(x => new { x.ErrorId });
            }
        }
    }

    public class PersistedExceptionLog
    {
        public Guid ErrorId { get; set; }
        public string Application { get; set; }
        public string Host { get; set; }
        public string Type { get; set; }
        public string Source { get; set; }
        public string Message { get; set; }
        public string User { get; set; }
        public int StatusCode { get; set; }
        public DateTime TimeUtc { get; set; }
        public int Sequence { get; set; }
        public string AllXml { get; set; }
    }

    public class PersistedExceptionLogRepository : ReadWriteRepository<PersistedExceptionLog, ElmahDbContext>, ICRUDRespoditory<PersistedExceptionLog>
    {
        public PersistedExceptionLogRepository(IAmbientDbContextLocator ambientDbContextLocator)
            : base(ambientDbContextLocator)
        {
        }

        public override async Task<PagedResult<PersistedExceptionLog>> FindAsync(Expression<Func<PersistedExceptionLog, bool>> predicate, IEnumerable<OrderByInfo<PersistedExceptionLog>> orderBy, int pageNumber, int pageSize)
        {
            var query = DbContext.Set<PersistedExceptionLog>().AsNoTracking().Where(predicate);
            var totalRecords = await query.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            if (pageNumber > totalPages) { pageNumber = totalPages; }
            if (totalRecords == 0)
                return new PagedResult<PersistedExceptionLog>(new List<PersistedExceptionLog>(), pageNumber, pageSize, 0);

            IOrderedQueryable<PersistedExceptionLog> orderedQuaQueryable = null;
            var orderByInfos = orderBy as OrderByInfo<PersistedExceptionLog>[] ?? orderBy.ToArray();
            for (var i = 0; i < orderByInfos.Count(); i++)
            {
                var info = orderByInfos[i];
                if (i == 0)
                {
                    orderedQuaQueryable = info.Ascending ? query.OrderBy(info.Property) : query.OrderByDescending(info.Property);
                }
                else
                {
                    if (orderedQuaQueryable != null)
                    {
                        orderedQuaQueryable = info.Ascending ? orderedQuaQueryable.ThenBy(info.Property) : orderedQuaQueryable.OrderByDescending(info.Property);
                    }
                }
            }
            if (orderedQuaQueryable == null)
                return new PagedResult<PersistedExceptionLog>(new List<PersistedExceptionLog>(), pageNumber, pageSize, 0);

            var rawdata = await orderedQuaQueryable.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            var data = rawdata.Select(x => new PersistedExceptionLog()
            {
                AllXml = string.Empty,
                ErrorId = x.ErrorId,
                Application = x.Application,
                Host = x.Host,
                Message = x.Message,
                Sequence = x.Sequence,
                Source = x.Source,
                StatusCode = x.StatusCode,
                TimeUtc = x.TimeUtc,
                Type = x.Type,
                User = x.User
            });
            return new PagedResult<PersistedExceptionLog>(data, pageNumber, pageSize, totalRecords);
        }
    }
}