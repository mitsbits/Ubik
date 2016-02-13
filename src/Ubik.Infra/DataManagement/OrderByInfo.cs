using System;
using System.Linq.Expressions;

namespace Ubik.Infra.DataManagement
{
    public struct OrderByInfo<T> where T : class
    {
        public Expression<Func<T, dynamic>> Property;
        public bool Ascending;

        public static OrderByInfo<TEntity> SortAscending<TEntity>(Expression<Func<TEntity, dynamic>> property) where TEntity : class
        {
            return new OrderByInfo<TEntity>() { Ascending = true, Property = property };
        }

        public static OrderByInfo<TEntity> SortDescending<TEntity>(Expression<Func<TEntity, dynamic>> property) where TEntity : class
        {
            return new OrderByInfo<TEntity>() { Ascending = false, Property = property };
        }
    }
}