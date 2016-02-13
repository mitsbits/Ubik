using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Ubik.Infra.DataManagement;

namespace Ubik.Infra.Contracts
{
    public interface IReadAsyncRepository<T> where T : class
    {
        Task<T> GetAsync(Expression<Func<T, bool>> predicate);

        Task<T> GetAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, dynamic>>[] paths);

        Task<PagedResult<T>> FindAsync(Expression<Func<T, bool>> predicate, IEnumerable<OrderByInfo<T>> orderBy, int pageNumber, int pageSize);

        Task<PagedResult<T>> FindAsync(Expression<Func<T, bool>> predicate, IEnumerable<OrderByInfo<T>> orderBy, int pageNumber, int pageSize, params Expression<Func<T, dynamic>>[] paths);

        Task<PagedResult<T>> FindAllAsync(Expression<Func<T, bool>> predicate, IEnumerable<OrderByInfo<T>> orderBy, params Expression<Func<T, dynamic>>[] paths);
    }
}