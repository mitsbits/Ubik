using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Ubik.Infra.Contracts;
using Ubik.Infra.DataManagement;
using Ubik.Infra.Ext;

namespace Ubik.EF6
{
    public abstract class BaseReadRepository<T, TDbContext> : IReadRepository<T>, IReadAsyncRepository<T>
        where T : class
        where TDbContext : DbContext
    {
        public abstract TDbContext DbContext { get; }

        public virtual IQueryable<T> GetQuery()
        {
            var entityName = GetEntitySetName(typeof(T));
            return ((IObjectContextAdapter)DbContext).ObjectContext.CreateQuery<T>(entityName);
        }

        public virtual T Get(Expression<Func<T, bool>> predicate)
        {
            return DbContext.Set<T>().FirstOrDefault(predicate);
        }

        public virtual T GetOriginalEntity(Expression<Func<T, bool>> predicate)
        {
            var t = Get(predicate);

            object originalItem;
            var entitySet = GetEntitySetName(typeof(T));
            var key = ((IObjectContextAdapter)DbContext).ObjectContext.CreateEntityKey(entitySet, t);

            if (((IObjectContextAdapter)DbContext).ObjectContext.TryGetObjectByKey(key, out originalItem))
            {
                return (T)originalItem;
            }
            return null;
        }

        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> predicate, Func<T, object> orderby)
        {
            return DbContext.Set<T>().Where(predicate).OrderBy(@orderby).ToList();
        }

        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> predicate, string orderByProperty, bool desc)
        {
            return DbContext.Set<T>().Where(predicate).OrderBy(orderByProperty, desc).ToList();
        }

        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> predicate, Func<T, object> orderby, bool desc, int pageNumber, int pageSize, out int totalRecords)
        {
            var q = DbContext.Set<T>().Where(predicate);
            totalRecords = q.Count();

            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            // in case the pageNumber is greater than totalPages
            // then we should use the last page to get the data
            if (pageNumber > totalPages) { pageNumber = totalPages; }

            return !desc ?
                q.DefaultIfEmpty().OrderBy(@orderby).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList() :
                q.DefaultIfEmpty().OrderByDescending(@orderby).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        }

        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> predicate, string orderByProperty, bool desc, int pageNumber, int pageSize, out int totalRecords)
        {
            var q = DbContext.Set<T>().Where(predicate);
            totalRecords = q.Count();

            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            // in case the pageNumber is greater than totalPages
            // then we should use the last page to get the data
            if (pageNumber > totalPages) { pageNumber = totalPages; }

            if (totalRecords == 0)  //a case with no data
            {
                return q.OrderBy(orderByProperty, desc);
            }

            return q.OrderBy(orderByProperty, desc).Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }

        public virtual bool Contains(Expression<Func<T, bool>> predicate)
        {
            return GetQuery().Any(predicate);
        }

        public virtual string GetEntitySetName(Type type)
        {
            var container = ((IObjectContextAdapter)DbContext)
                .ObjectContext.MetadataWorkspace.GetEntityContainer(((IObjectContextAdapter)DbContext).ObjectContext.DefaultContainerName, DataSpace.CSpace);
            return (from meta in container.BaseEntitySets
                    where meta.ElementType.Name == type.Name
                    select meta.Name).First();
        }

        #region Async

        public virtual async Task<T> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await DbContext.Set<T>().FirstOrDefaultAsync(predicate);
        }

        public virtual async Task<T> GetAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, dynamic>>[] paths)
        {
            var query = DbContext.Set<T>().Where(predicate);
            if (paths != null && paths.Any())
            {
                query = paths.Aggregate(query, (current, path) => current.Include(path));
            }
            return await query.FirstOrDefaultAsync();
        }

        public virtual async Task<PagedResult<T>> FindAsync(Expression<Func<T, bool>> predicate,
            IEnumerable<OrderByInfo<T>> orderBy, int pageNumber, int pageSize)
        {
            return await FindAsync(predicate, orderBy, pageNumber, pageSize, null);
        }

        public async Task<PagedResult<T>> FindAsync(Expression<Func<T, bool>> predicate,
            IEnumerable<OrderByInfo<T>> orderBy, int pageNumber, int pageSize, params Expression<Func<T, dynamic>>[] paths)
        {
            var query = DbContext.Set<T>().Where(predicate);
            var totalRecords = await query.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            if (pageNumber > totalPages) { pageNumber = totalPages; }
            if (totalRecords == 0)
                return new PagedResult<T>(new List<T>(), pageNumber, pageSize, 0);

            if (paths != null && paths.Any())
            {
                query = paths.Aggregate(query, (current, path) => current.Include(path));
            }

            IOrderedQueryable<T> orderedQuaQueryable = null;
            var orderByInfos = orderBy as OrderByInfo<T>[] ?? orderBy.ToArray();
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
                return new PagedResult<T>(new List<T>(), pageNumber, pageSize, 0);

            var data = await orderedQuaQueryable.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PagedResult<T>(data, pageNumber, pageSize, totalRecords);
        }

        public async Task<PagedResult<T>> FindAllAsync(Expression<Func<T, bool>> predicate,
                    IEnumerable<OrderByInfo<T>> orderBy, params Expression<Func<T, dynamic>>[] paths)
        {
            var query = DbContext.Set<T>().Where(predicate);

            if (paths != null && paths.Any())
            {
                query = paths.Aggregate(query, (current, path) => current.Include(path));
            }

            IOrderedQueryable<T> orderedQuaQueryable = null;
            var orderByInfos = orderBy as OrderByInfo<T>[] ?? orderBy.ToArray();
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
                return new PagedResult<T>(new List<T>(), 1, 1, 0);

            var data = await orderedQuaQueryable.ToListAsync();
            var totalRecords = data.Count();
            return new PagedResult<T>(data, 1, totalRecords, totalRecords);
        }

        #endregion Async
    }
}