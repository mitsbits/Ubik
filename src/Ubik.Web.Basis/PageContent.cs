using Ubik.Web.Basis.Contracts;

namespace Ubik.Web.Basis
{
    public class PageContent : IPageContent
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }

        public string[] Body { get; set; }
    }
}