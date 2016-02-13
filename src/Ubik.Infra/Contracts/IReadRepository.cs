using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Ubik.Infra.Contracts
{
    public interface IReadRepository<T> where T : class
    {
        /// <summary>
        /// Gets an IQuerable strongly typed.
        /// </summary>
        /// <returns><c>IQueryable</c>.</returns>
        IQueryable<T> GetQuery();

        /// <summary>
        /// Gets a POCO object using the filter expression.
        /// </summary>
        /// <param name="predicate">The predicate expression for filtering resultd.</param>
        /// <returns>POCO object</returns>
        T Get(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Gets the original POCO entity before any updares are commited to the object.
        /// </summary>
        /// <param name="predicate">The predicate expression to filter results.</param>
        /// <returns>POCO entity</returns>
        T GetOriginalEntity(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Returns a collection of POCO objects filtered and sorted.
        /// </summary>
        /// <param name="predicate">The predicate expression to filter results.</param>
        /// <param name="orderby">The orderby expression to sort results.</param>
        /// <returns>An <c>IEnumerable</c> of POCO entities</returns>
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate, Func<T, object> orderby);

        /// <summary>
        /// Returns a collection of POCO entities filtered and sorted.
        /// </summary>
        /// <param name="predicate">The predicate expression to filter results.</param>
        /// <param name="orderByProperty">The field name to preform the sort operation on.</param>
        /// <param name="desc">if set to <c>true</c> the results is sorted descending, else ascending.</param>
        /// <returns>An <c>IEnumerable</c> of POCO entities.</returns>
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate, string orderByProperty, bool desc);

        /// <summary>
        /// Returns a collection of POCO objects filtered and sorted.
        /// Supports paging.
        /// </summary>
        /// <param name="predicate">The predicate expression to filter results.</param>
        /// <param name="orderby">The orderby expression to sort results.</param>
        /// <param name="desc">if set to <c>true</c> returns entities in descending order.</param>
        /// <param name="pageNumber">The page number, 1 based.</param>
        /// <param name="pageSize">The count of results per page.</param>
        /// <param name="totalRecords">The total number of records returnd by the filtered query.</param>
        /// <returns>An <c>IEnumerable</c> of POCO entities.</returns>
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate, Func<T, object> orderby, bool desc, int pageNumber, int pageSize, out int totalRecords);

        /// <summary>
        /// Returns a collection of POCO objects filtered and sorted.
        /// Supports paging.
        /// </summary>
        /// <param name="predicate">The predicate expression to filter results.</param>
        /// <param name="orderByProperty">The field name to preform the sort operation on.</param>
        /// <param name="desc">if set to <c>true</c> the results is sorted descending, else ascending.</param>
        /// <param name="pageNumber">The page number, 1 based.</param>
        /// <param name="pageSize">The count of results per page.</param>
        /// <param name="totalRecords">The total number of records returnd by the filtered query.</param>
        /// <returns>An <c>IEnumerable</c> of POCO entities.</returns>
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate, string orderByProperty, bool desc, int pageNumber, int pageSize, out int totalRecords);

        /// <summary>
        /// Determines whether a POCO entity exists in the data store based on the filter expressions.
        /// </summary>
        /// <param name="predicate">The predicate expression to filter results.</param>
        /// <returns>
        ///   <c>true</c> if a POCO entity exists in the data store based in the specified predicate; otherwise, <c>false</c>.
        /// </returns>
        bool Contains(Expression<Func<T, bool>> predicate);
    }
}