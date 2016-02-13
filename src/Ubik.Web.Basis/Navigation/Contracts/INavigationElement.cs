using Ubik.Infra.Contracts;

namespace Ubik.Web.Basis.Navigation.Contracts
{
    public interface INavigationElement<out TKey> : IHierarchyData, IWeighted, IHasParent<TKey> where TKey : struct
    {
        NavigationElementRole Role { get; }

        /// <summary>
        /// Gets or sets the anchor target.
        /// </summary>
        /// <value>
        /// The anchor target.
        /// </value>
        string AnchorTarget { get; }

        /// <summary>
        /// Gets or sets the display content of the element.
        /// </summary>
        /// <value>
        /// The display content.
        /// </value>
        string Display { get; }

        /// <summary>
        /// Gets or sets the href value of the navigational element.
        /// </summary>
        /// <value>
        /// The href value.
        /// </value>
        string Href { get; }

        int Depth { get; }

        INavigationGroup Group { get; }

        string IconCssClass { get; }
    }
}