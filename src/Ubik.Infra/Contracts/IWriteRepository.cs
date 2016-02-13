using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Ubik.Infra.Contracts
{
    public interface IWriteRepository<T> where T : class
    {
        /// <summary>
        /// Creates or updates an object in the datastore if the object is not found,
        /// </summary>
        /// <param name="entity">The POCO entity.</param>
        void CreateOrUpdate(T entity);

        /// <summary>
        /// Deletes the specified POCO entity.
        /// </summary>
        /// <param name="entity">The POCO entity.</param>
        void Delete(T entity);

        /// <summary>
        /// Deletes the specified POCO according with the predicate value.
        /// </summary>
        /// <param name="predicate">The predicate expression for filtering results</param>
        void Delete(Expression<Func<T, bool>> predicate);
    }

    public interface IWriteAsyncRepository<T> where T : class
    {
        Task<T> CreateAsync(T entity);

        Task<T> UpdateAsync(T entity);

        Task DeleteAsync(Expression<Func<T, bool>> predicate);
    }
}