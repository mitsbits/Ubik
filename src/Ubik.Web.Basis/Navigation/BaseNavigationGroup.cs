using Ubik.Web.Basis.Navigation.Contracts;

namespace Ubik.Web.Basis.Navigation
{
    public class BaseNavigationGroup : INavigationGroup
    {
        public string Display { get; set; }
        public string Key { get; set; }
        public string Description { get; set; }
        public double Weight { get; set; }
    }
}