using System;
using System.Collections.Generic;
using System.Linq;
using Ubik.Infra.Contracts;
using Ubik.Web.Basis.Navigation.Contracts;

namespace Ubik.Web.Basis.Navigation
{
    public class BaseNavigationElement : INavigationElement<int>, IHasParent
    {
        private const string PathSeperator = @".";
        private readonly IEnumerable<NavigationElementDto> _data;
        private readonly NavigationElementDto _proxy;
        private readonly int _depth;

        public BaseNavigationElement(IEnumerable<NavigationElementDto> data, int id)
        {
            _data = data as NavigationElementDto[] ?? data.ToArray();
            if (Data.All(x => x.Id != id)) throw new NullReferenceException("proxy");
            _proxy = Data.FirstOrDefault(x => x.Id == id);
            _depth = CalculateDepth();
        }

        private int CalculateDepth()
        {
            var d = 1;
            var id = ParentId;
            while ((id > 0))
            {
                d++;
                id = Convert.ToInt32(Data.FirstOrDefault(x => x.Id == id).ParentId);
            }
            return d;
        }

        public NavigationElementRole Role => Proxy.Role;

        public string AnchorTarget => Proxy.AnchorTarget;

        public string Display => Proxy.Display;

        public string Href => Proxy.Href;

        public int Depth => _depth;

        public INavigationGroup Group => new BaseNavigationGroup() { Description = Proxy.Group.Description, Display = Proxy.Group.Display, Key = Proxy.Group.Key, Weight = Proxy.Group.Weight };

        public string IconCssClass => Proxy.IconCssClass;

        public bool HasChildren
        {
            get { return Data.Any(x => x.ParentId == Proxy.Id); }
        }

        public object Item => this;

        public string Type => GetType().FullName;

        protected virtual IHierarchicalEnumerable GetChildren()
        {
            var result = new BaseNavigationElements<BaseNavigationElement>(
                Data.Where(x => x.ParentId == Proxy.Id)
                .OrderBy(x => x.Weight)
                .Select(x => new BaseNavigationElement(Data, x.Id))
                .ToList());
            return result;
        }

        public IHierarchicalEnumerable Children => GetChildren();

        protected virtual IHierarchyData GetParent()
        {
            return Data.Any(x => x.Id == Proxy.ParentId) ? new BaseNavigationElement(Data, Proxy.ParentId) : null;
        }

        public IHierarchyData Parent => GetParent();

        public double Weight => Proxy.Weight;

        public int Id => Proxy.Id;

        public int ParentId => Proxy.ParentId;

        protected IEnumerable<NavigationElementDto> Data => _data;

        protected NavigationElementDto Proxy => _proxy;
    }
}