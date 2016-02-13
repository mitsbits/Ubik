using System.Collections.Generic;
using Ubik.Web.Components.Contracts;

namespace Ubik.Web.Components.Domain
{
    public class Content<TKey> : ComponentBase<TKey>, IContent, IHasTags
    {
        protected Content()
        {
        }

        public Content(TKey id, Textual textual, string canonicalUrl)
            : base(id)
        {
            Textual = textual;
            HtmlHead = new HtmlHead(canonicalUrl);
            Tags = new HashSet<Tag<TKey>>();
        }

        ITextualInfo IContent.Textual
        {
            get { return Textual; }
        }

        public Textual Textual { get; private set; }

        IHtmlHeadInfo IContent.HtmlHeadInfo
        {
            get { return HtmlHead; }
        }

        public HtmlHead HtmlHead { get; private set; }

        IEnumerable<ITag> IHasTags.Tags
        {
            get { return Tags; }
        }

        public ICollection<Tag<TKey>> Tags { get; private set; }
    }

    public class Tag<TKey> : ComponentBase<TKey>, ITag
    {
        protected Tag()
        {
        }

        public Tag(TKey id, string value) : base(id)
        {
            Value = value;
        }

        public string Value { get; private set; }
    }
}