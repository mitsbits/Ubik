using System.Collections;

namespace Ubik.Infra.Contracts
{
    /// <summary>
    /// Represents a hierarchical collection that can be enumerated with an <see cref="T:System.Collections.IEnumerator"/> interface. Collections that implement the <see cref="T:System.Web.UI.IHierarchicalEnumerable"/> interface are used by ASP.NET site navigation and data source controls.
    /// </summary>
    public interface IHierarchicalEnumerable : IEnumerable
    {
        /// <summary>
        /// Returns a hierarchical data item for the specified enumerated item.
        /// </summary>
        ///
        /// <returns>
        /// An <see cref="T:System.Web.UI.IHierarchyData"/> instance that represents the <see cref="T:System.Object"/> passed to the <see cref="M:System.Web.UI.IHierarchicalEnumerable.GetHierarchyData(System.Object)"/> method.
        /// </returns>
        /// <param name="enumeratedItem">The <see cref="T:System.Object"/> for which to return an <see cref="T:System.Web.UI.IHierarchyData"/>. </param>
        IHierarchyData GetHierarchyData(object enumeratedItem);
    }
}