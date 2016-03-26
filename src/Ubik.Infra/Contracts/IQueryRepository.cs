using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Ubik.Infra.DataManagement;

namespace Ubik.Infra.Contracts
{
    public interface IQueryRepository<TEntity, TProjection> : IQueryRepository<TProjection>
        where TEntity : class
        where TProjection : class
    {
        Task<IEnumerable<TEntity>> FindMapedAsync(Expression<Func<TProjection, bool>> predicate, IEnumerable<OrderByInfo<TEntity>> orderBy);
    }

    public interface IQueryRepository<TProjection>
        where TProjection : class
    {
        Task<PagedResult<TProjection>> FindAsync(Expression<Func<TProjection, bool>> predicate, IEnumerable<OrderByInfo<TProjection>> orderBy, int pageNumber, int pageSize);
    }
}