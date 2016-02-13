using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ubik.Infra.Contracts;

namespace Ubik.Infra.DataManagement
{
    public class PagedResult<T> : IPagedResult<T>
    {
        #region Declarations

        private readonly List<T> _data = new List<T>();

        #endregion Declarations

        #region Public Constructor

        public PagedResult(IEnumerable<T> data, int pageNumber, int pageSize, int totalRecords)
            : this()
        {
            this.Clear();
            this.AddRange(data);
            this.TotalRecords = totalRecords;
            this.PageSize = pageSize;
            this.PageNumber = Math.Min(Math.Max(1, pageNumber), TotalPages);
        }

        public PagedResult()
        {
            this.Clear();

            this.TotalRecords = 0;
            this.PageSize = 30;
            this.PageNumber = Math.Min(Math.Max(1, PageSize), TotalPages);
        }

        #endregion Public Constructor

        #region IPagedList implementation

        /// <summary>
        /// Gets the page number.
        /// </summary>
        public int PageNumber { get; private set; }

        /// <summary>
        /// Gets the size of the page.
        /// </summary>
        /// <value>
        /// The size of the page.
        /// </value>
        public int PageSize { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance has previous page.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has previous page; otherwise, <c>false</c>.
        /// </value>
        public bool HasPreviousPage { get { return (PageNumber > 1); } }

        /// <summary>
        /// Gets a value indicating whether this instance has next page.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has next page; otherwise, <c>false</c>.
        /// </value>
        public bool HasNextPage { get { return (PageNumber * PageSize) < TotalRecords; } }

        /// <summary>
        /// Gets the total pages.
        /// </summary>
        public int TotalPages { get { return (int)Math.Ceiling((double)TotalRecords / PageSize); } }

        /// <summary>
        /// Gets the total records.
        /// </summary>
        public int TotalRecords { get; private set; }

        /// <summary>
        /// Gets the data.
        /// </summary>
        public IList<T> Data
        {
            get { return _data; }
        }

        #endregion IPagedList implementation

        #region List implementation

        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <param name="range">The range.</param>
        public void AddRange(IEnumerable<T> range)
        {
            this._data.AddRange(range);
        }

        /// <summary>
        /// Indexes the of.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public int IndexOf(T item)
        {
            return _data.IndexOf(item);
        }

        /// <summary>
        /// Inserts the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="item">The item.</param>
        public void Insert(int index, T item)
        {
            _data.Insert(index, item);
        }

        /// <summary>
        /// Removes at.
        /// </summary>
        /// <param name="index">The index.</param>
        public void RemoveAt(int index)
        {
            _data.RemoveAt(index);
        }

        /// <summary>
        /// Gets or sets the <see cref="T"/> at the specified index.
        /// </summary>
        public T this[int index]
        {
            get
            {
                return _data[index];
            }
            set
            {
                _data[index] = value;
            }
        }

        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Add(T item)
        {
            _data.Add(item);
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            _data.Clear();
        }

        /// <summary>
        /// Determines whether [contains] [the specified item].
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>
        ///   <c>true</c> if [contains] [the specified item]; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(T item)
        {
            return _data.Contains(item);
        }

        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="arrayIndex">Index of the array.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            _data.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        public int Count
        {
            get { return _data.Count(); }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is read only.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is read only; otherwise, <c>false</c>.
        /// </value>
        public bool IsReadOnly
        {
            get { return true; }
        }

        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public bool Remove(T item)
        {
            return _data.Remove(item);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return ((IEnumerable<T>)_data).GetEnumerator();
        }

        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable)_data).GetEnumerator();
        }

        #endregion List implementation
    }
}