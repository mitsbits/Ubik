using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Razor;
using Microsoft.AspNet.Mvc.Rendering;
using Ubik.Web.Basis.Helpers;

namespace Ubik.Web.Client.Backoffice
{
    public abstract class BackofficePage : RazorPage
    {
        protected BackofficePage()
            : base()
        {
        }

        private BackofficeContentHelper _pageContent;

        public BackofficeContentHelper PageContent
        {
            get
            {
                if (_pageContent == null) _pageContent = new BackofficeContentHelper(ViewContext);
                return _pageContent;
            }
        }

        private ServerResponseHelper _feedback;

        public ServerResponseHelper Feedback
        {
            get
            {
                if (_feedback == null) _feedback = new ServerResponseHelper(ViewContext);
                return _feedback;
            }
        }

        public void AddBackofficeBottom(string urlstring, IUrlHelper url, IHtmlHelper html)
        {
            const string prefix = @"~/Areas/Backoffice/Scripts/framework/";
            urlstring = urlstring.TrimStart('/');

            var path = string.Format("{0}{1}", url.Content(prefix), (urlstring.EndsWith(".js") ? urlstring : urlstring + ".js"));
            html.Statics().FooterScripts.Add(path);
        }
    }

    public abstract class BackofficePage<TModel> : RazorPage<TModel>
    {
        protected BackofficePage()
            : base()
        {
        }

        private BackofficeContentHelper _pageContent;

        public BackofficeContentHelper PageContent
        {
            get
            {
                if (_pageContent == null) _pageContent = new BackofficeContentHelper(ViewContext);
                return _pageContent;
            }
        }

        private ServerResponseHelper _feedback;

        public ServerResponseHelper Feedback
        {
            get
            {
                if (_feedback == null) _feedback = new ServerResponseHelper(ViewContext);
                return _feedback;
            }
        }

        public void AddBackofficeBottom(string urlstring, IUrlHelper url, IHtmlHelper html)
        {
            const string prefix = @"~/Areas/Backoffice/Scripts/framework/";
            urlstring = urlstring.TrimStart('/');

            var path = string.Format("{0}{1}", url.Content(prefix), (urlstring.EndsWith(".js") ? urlstring : urlstring + ".js"));
            html.Statics().FooterScripts.Add(path);
        }
    }
}