namespace Ubik.Web.Basis.Navigation
{
    public struct NavigationGroupDto
    {
        public string Key { get; set; }
        public string Display { get; set; }
        public string Description { get; set; }
        public double Weight { get; set; }
        public string IconCssClass { get; set; }

        public static NavigationGroupDto Empty()
        {
            return new NavigationGroupDto() { Display = string.Empty, Description = string.Empty, Key = string.Empty };
        }

        public static NavigationGroupDto WithKey(string key, string display)
        {
            return new NavigationGroupDto() { Display = display, Description = string.Empty, Key = key };
        }
    }
}