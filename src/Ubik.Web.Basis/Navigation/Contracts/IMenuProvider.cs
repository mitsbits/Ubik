using System.Collections.Generic;

namespace Ubik.Web.Basis.Navigation.Contracts
{
    public interface IMenuProvider<out TCollection> where TCollection : INavigationElements<int>
    {
        ICollection<NavigationElementDto> Raw { get; }
        TCollection Menu { get; }
    }
}