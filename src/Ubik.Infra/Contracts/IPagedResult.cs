using System.Collections.Generic;

namespace Ubik.Infra.Contracts
{
    public interface IPagedResult<T> : IList<T>
    {
        /// <summary>
        /// Gets the data.
        /// </summary>
        IList<T> Data { get; }

        /// <summary>
        /// Gets the page number.
        /// </summary>
        int PageNumber { get; }

        /// <summary>
        /// Gets a value indicating whether this instance has next page.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has next page; otherwise, <c>false</c>.
        /// </value>
        bool HasNextPage { get; }

        /// <summary>
        /// Gets a value indicating whether this instance has previous page.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has previous page; otherwise, <c>false</c>.
        /// </value>
        bool HasPreviousPage { get; }

        /// <summary>
        /// Gets the size of the page.
        /// </summary>
        /// <value>
        /// The size of the page.
        /// </value>
        int PageSize { get; }

        /// <summary>
        /// Gets the total records.
        /// </summary>
        int TotalRecords { get; }

        /// <summary>
        /// Gets the total pages.
        /// </summary>
        int TotalPages { get; }
    }
}