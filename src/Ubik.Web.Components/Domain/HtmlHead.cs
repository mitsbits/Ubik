using System;
using System.Collections.Generic;
using Ubik.Web.Components.Contracts;

namespace Ubik.Web.Components.Domain
{

    public class HtmlHead : IHtmlHeadInfo
    {
        public HtmlHead()
        {
            CanonicalURL = string.Empty;
            Slug = string.Empty;
            Metas = new HashSet<Meta>();
        }

        public HtmlHead(string canonicalUrl)
            : this()
        {
            CanonicalURL = canonicalUrl;
        }

        public string CanonicalURL
        {
            get;
            private set;
        }

        public string Slug
        {
            get;
            private set;
        }

        IEnumerable<IHtmlMeta> IHtmlHeadInfo.Metas
        {
            get { return Metas; }
        }

        public ICollection<Meta> Metas
        {
            get;
            private set;
        }

        public void SetSlug(string slug)
        {
            Slug = slug;
        }

        public void SetCanonicalURL(string canonicalURL)
        {
            CanonicalURL = canonicalURL;
        }
    }
}