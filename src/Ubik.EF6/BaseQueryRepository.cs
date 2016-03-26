using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Ubik.Infra.Contracts;
using Ubik.Infra.DataManagement;

namespace Ubik.EF6
{
    public abstract class BaseQueryRepository<TProjection, TDbContext> : IQueryRepository<TProjection>
    where TProjection : class
    where TDbContext : DbContext
    {
        public abstract TDbContext DbContext { get; }

        public async Task<PagedResult<TProjection>> FindAsync(Expression<Func<TProjection, bool>> predicate, IEnumerable<OrderByInfo<TProjection>> orderBy, int pageNumber, int pageSize)
        {
            var set = DbContext.Set<TProjection>().AsNoTracking();
            var query = set.Where(predicate);
            var totalRecords = await query.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            if (pageNumber > totalPages) { pageNumber = totalPages; }
            if (totalRecords == 0)
                return new PagedResult<TProjection>(new List<TProjection>(), pageNumber, pageSize, 0);
            IOrderedQueryable<TProjection> orderedQueryable = null;
            var orderByInfos = orderBy as OrderByInfo<TProjection>[] ?? orderBy.ToArray();
            for (var i = 0; i < orderByInfos.Count(); i++)
            {
                var info = orderByInfos[i];
                if (i == 0)
                {
                    orderedQueryable = info.Ascending ? query.OrderBy(info.Property) : query.OrderByDescending(info.Property);
                }
                else
                {
                    if (orderedQueryable != null)
                    {
                        orderedQueryable = info.Ascending ? orderedQueryable.ThenBy(info.Property) : orderedQueryable.OrderByDescending(info.Property);
                    }
                }
            }
            if (orderedQueryable == null)
                return new PagedResult<TProjection>(new List<TProjection>(), pageNumber, pageSize, 0);

            var data = await orderedQueryable.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PagedResult<TProjection>(data, pageNumber, pageSize, totalRecords);
        }
    }

    public abstract class BaseQueryRepository<TEntity, TProjection, TDbContext> : BaseQueryRepository<TProjection, TDbContext>, IQueryRepository<TEntity, TProjection>
    where TEntity : class
    where TProjection : class
    where TDbContext : DbContext
    {
        public async Task<IEnumerable<TEntity>> FindMapedAsync(Expression<Func<TProjection, bool>> predicate, IEnumerable<OrderByInfo<TEntity>> orderBy)
        {
            var set = DbContext.Set<TProjection>().AsNoTracking();
            var query = set.Where(predicate);
            var rows = await query.ToListAsync();
            var entities = Map(rows).AsQueryable();
            IOrderedQueryable<TEntity> orderedQueryable = null;
            var orderByInfos = orderBy as OrderByInfo<TEntity>[] ?? orderBy.ToArray();
            for (var i = 0; i < orderByInfos.Count(); i++)
            {
                var info = orderByInfos[i];
                if (i == 0)
                {
                    orderedQueryable = info.Ascending ? entities.OrderBy(info.Property) : entities.OrderByDescending(info.Property);
                }
                else
                {
                    if (orderedQueryable != null)
                    {
                        orderedQueryable = info.Ascending ? orderedQueryable.ThenBy(info.Property) : orderedQueryable.OrderByDescending(info.Property);
                    }
                }
            }
            if (orderedQueryable == null) return new List<TEntity>();
            return await orderedQueryable.ToListAsync();
        }

        protected abstract IEnumerable<TEntity> Map(IEnumerable<TProjection> source);
    }
}