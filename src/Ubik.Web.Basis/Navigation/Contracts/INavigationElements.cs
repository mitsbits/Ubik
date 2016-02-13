using System.Collections.Generic;
using Ubik.Infra.Contracts;

namespace Ubik.Web.Basis.Navigation.Contracts
{
    /// <summary>
    /// <see>
    ///     <cref>INavigationElements</cref>
    /// </see>
    ///     is a contract for an hierachical collection of naviagation items.
    /// </summary>
    public interface INavigationElements<in TKey> : IHierarchicalEnumerable
        where TKey : struct
    {
    }

    public interface INavigationElementContainer<in TKey, out TElement>
        where TKey : struct
        where TElement : INavigationElement<TKey>
    {
        IReadOnlyCollection<TElement> Data { get; }
    }
}