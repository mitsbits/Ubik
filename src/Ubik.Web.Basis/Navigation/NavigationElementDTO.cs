namespace Ubik.Web.Basis.Navigation
{
    public class NavigationElementDto
    {
        public int Id { get; set; }

        public int ParentId { get; set; }

        public NavigationElementRole Role { get; set; }

        public string AnchorTarget { get; set; }

        public string Display { get; set; }

        public string Href { get; set; }

        public NavigationGroupDto Group { get; set; }

        public double Weight { get; set; }

        public string IconCssClass { get; set; }
    }
}