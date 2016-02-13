using System;
using Ubik.Web.Components.Contracts;

namespace Ubik.Web.Components.Domain
{
    public class Meta : IHtmlMeta
    {
        public virtual string Content
        {
            get;
            set;
        }

        public virtual string HttpEquiv
        {
            get;
            set;
        }

        public virtual string Name
        {
            get;
            set;
        }

        public virtual string Scheme
        {
            get;
            set;
        }

        public virtual bool ShouldRender
        {
            get
            {
                return !string.IsNullOrWhiteSpace(Content)
                    && (!string.IsNullOrWhiteSpace(Name)
                    || !string.IsNullOrWhiteSpace(HttpEquiv));
            }
        }

        public virtual bool IsHttpEquiv
        {
            get { return !string.IsNullOrWhiteSpace(HttpEquiv); }
        }

        public virtual bool IsOpenGraph
        {
            get
            {
                return (!string.IsNullOrWhiteSpace(Name)
                    && Name.StartsWith("og:", StringComparison.CurrentCultureIgnoreCase));
            }
        }

        public virtual bool IsTwitterCard
        {
            get
            {
                return (!string.IsNullOrWhiteSpace(Name)
                    && Name.StartsWith("twitter:", StringComparison.CurrentCultureIgnoreCase));
            }
        }

        public virtual string TypeIdentifier
        {
            get
            {
                if (IsOpenGraph) return "OPENGRAPH";
                if (IsTwitterCard) return "TWITTERCARD";
                return IsHttpEquiv ? "HTTPEQUIV" : "BASIC";
            }
        }
    }
}