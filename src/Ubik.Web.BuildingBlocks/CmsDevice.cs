using System.Collections.Generic;
using Ubik.Web.Components;
using Ubik.Web.Components.Contracts;

namespace Ubik.Web.BuildingBlocks
{
    public class CmsDevice : IDevice
    {
        private readonly ICollection<ISection> _sections;

        public CmsDevice()
        {
            _sections = new HashSet<ISection>();
        }

        public string FriendlyName { get; set; }

        public string Path { get; set; }

        public ICollection<ISection> Sections
        {
            get { return _sections; }
        }

        public DeviceRenderFlavor Flavor { get; set; }
    }

    public class CmsContent : IContent
    {
        private readonly ICollection<CmsHtmlMeta> _metas;

        public CmsContent()
        {
            _metas = new HashSet<CmsHtmlMeta>();
        }

        public ITextualInfo Textual { get; private set; }
        public IHtmlHeadInfo HtmlHeadInfo { get; private set; }
        public string CanonicalURL { get; private set; }
        public string Slug { get; private set; }

        //IEnumerable<IHtmlMeta> IContent.Metas { get { return Metas; } }
        internal ICollection<CmsHtmlMeta> Metas { get { return _metas; } }

        protected IEnumerable<IHtmlMeta> DbMetas { get; set; }
    }
}