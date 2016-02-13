using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Ubik.Infra.DataManagement.Group;

namespace Ubik.Infra.Ext
{
    public static class CollectionExtensions
    {
        public static IList<T> Shuffle<T>(this IList<T> list)
        {
            var rng = new Random();
            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = rng.Next(n + 1);
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
            return list;
        }

        public static IEnumerable<T> Traverse<T>(this IEnumerable<T> items,
            Func<T, IEnumerable<T>> childSelector)
        {
            var stack = new Stack<T>(items);
            while (stack.Any())
            {
                var next = stack.Pop();
                yield return next;
                foreach (var child in childSelector(next))
                    stack.Push(child);
            }
        }

        /// <summary>
        /// Orders the provided <see cref="IQueryable"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="orderByProperty">The order by property.</param>
        /// <param name="desc">if set to <c>true</c> [desc].</param>
        /// <returns></returns>
        public static IQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> source, string orderByProperty,
                bool desc) where TEntity : class
        {
            var command = desc ? "OrderByDescending" : "OrderBy";

            var type = typeof(TEntity);

            var property = type.GetProperty(orderByProperty);

            var parameter = Expression.Parameter(type, "p");

            var propertyAccess = Expression.MakeMemberAccess(parameter, property);

            var orderByExpression = Expression.Lambda(propertyAccess, parameter);

            var resultExpression = Expression.Call(typeof(Queryable), command, new Type[] { type, property.PropertyType },

                                   source.Expression, Expression.Quote(orderByExpression));

            return source.Provider.CreateQuery<TEntity>(resultExpression);
        }

        #region Groups

        /// <summary>
        /// Group a collection based on a key
        /// and calculate an aggregate for each group.
        /// </summary>
        /// <typeparam name="TSource">The collection type</typeparam>
        /// <typeparam name="TValue">The aggreagted value type</typeparam>
        /// <param name="collection">The Enumerable to group against</param>
        /// <param name="groupBy">The group by predicate to generate the Key</param>
        /// <param name="getValue">The predicate to calculate the group agregated Value</param>
        /// <example>Sum persons ages based on sex -  var groups = collection.ToGroups(person => new { person.Sex }, persons => persons.Sum(person => person.Age));</example>
        /// <returns>A dictionary with the group Key and the grou aggregated Value /></returns>
        public static IDictionary<object, TValue> ToGroups<TSource, TValue>(this IEnumerable<TSource> collection,
            Func<TSource, dynamic> groupBy, Func<IEnumerable<TSource>, TValue> getValue)
        {
            var data = collection.ToArray();
            var service = GroupingHelper<TSource, TValue>.CreateGroupService(groupBy, getValue);
            return service.Group(data);
        }

        public static IDictionary<int, TSource> ItemsThatSatisfyGroupThreshold<TSource, TValue>(
            this IEnumerable<TSource> collection,
            Func<TSource, dynamic> groupBy, Func<IEnumerable<TSource>, TValue> getValue, Func<TValue, bool> validator)
        {
            return FilterItemsBasedOnGroup(collection, groupBy, getValue, validator);
        }

        public static IDictionary<int, TSource> ItemsThatDoNotSatisfyGroupThreshold<TSource, TValue>(
            this IEnumerable<TSource> collection,
            Func<TSource, dynamic> groupBy, Func<IEnumerable<TSource>, TValue> getValue, Func<TValue, bool> validator)
        {
            return FilterItemsBasedOnGroup(collection, groupBy, getValue, validator, false);
        }

        private static IDictionary<int, TSource> FilterItemsBasedOnGroup<TSource, TValue>(
            this IEnumerable<TSource> collection,
            Func<TSource, dynamic> groupBy, Func<IEnumerable<TSource>, TValue> getValue, Func<TValue, bool> validator, bool fetchValid = true)
        {
            var data = collection.ToArray();
            var service = GroupingHelper<TSource, TValue>.CreateGroupValidationService(data, groupBy, getValue, validator);
            var result = new Dictionary<int, TSource>();
            for (var i = 0; i < data.Count(); i++)
            {
                var item = data[i];
                if ((fetchValid && service.Validate(item)) || (!fetchValid && !service.Validate(item))) result.Add(i, item);
            }
            return result;
        }

        #endregion Groups
    }
}