using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Ubik.Infra.Contracts
{
    public interface IQueryRepository<out TEntity, TProjection>
        where TEntity : class
        where TProjection : class
    {
        IEnumerable<TEntity> Find(Expression<Func<TProjection, bool>> predicate, Func<TProjection, object> orderby, bool desc, int pageNumber, int pageSize, out int totalRecords);
    }
}