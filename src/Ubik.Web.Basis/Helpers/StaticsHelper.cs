using Microsoft.AspNet.Html.Abstractions;
using Microsoft.AspNet.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//code from: https://github.com/speier/mvcassetshelper

namespace Ubik.Web
{
    public static class HtmlHelperExtensions
    {
        public static StaticsHelper Statics(this IHtmlHelper htmlHelper)
        {
            return StaticsHelper.GetInstance(htmlHelper);
        }
    }

    public class StaticsHelper
    {
        public static StaticsHelper GetInstance(IHtmlHelper htmlHelper)
        {
            const string instanceKey = "8F3AC534-E5B1-4C51-B5A9-ED0EAC731633";

            var context = htmlHelper.ViewContext.HttpContext;
            if (context == null) return null;

            var assetsHelper = (StaticsHelper)context.Items[instanceKey];

            if (assetsHelper == null)
                context.Items.Add(instanceKey, assetsHelper = new StaticsHelper());

            return assetsHelper;
        }

        public ItemRegistrar Styles { get; private set; }
        public ItemRegistrar HeadScripts { get; private set; }
        public ItemRegistrar FooterScripts { get; private set; }
        public ItemRegistrar InlineScripts { get; private set; }
        public ItemRegistrar InlineStyles { get; private set; }

        public StaticsHelper()
        {
            Styles = new ItemRegistrar(ItemRegistrarFormatters.StyleFormat);
            HeadScripts = new ItemRegistrar(ItemRegistrarFormatters.ScriptFormat);
            FooterScripts = new ItemRegistrar(ItemRegistrarFormatters.ScriptFormat);
            InlineScripts = new ItemRegistrar(ItemRegistrarFormatters.InlineScripts);
            InlineStyles = new ItemRegistrar(ItemRegistrarFormatters.InlineStyles);
        }
    }

    public class ItemRegistrar
    {
        private readonly string _format;
        private readonly IList<string> _items;

        public ItemRegistrar(string format)
        {
            _format = format;
            _items = new List<string>();
        }

        public ItemRegistrar Add(string url)
        {
            if (!_items.Contains(url))
                _items.Add(url);

            return this;
        }

        public IHtmlContent Render()
        {
            var sb = new StringBuilder();

            foreach (var fmt in _items.Select(item => string.Format(_format, item)))
            {
                sb.AppendLine(fmt);
            }

            return new HtmlString(sb.ToString());
        }
    }

    public class ItemRegistrarFormatters
    {
        public const string StyleFormat = "<link href=\"{0}\" rel=\"stylesheet\" type=\"text/css\" />";
        public const string ScriptFormat = "<script src=\"{0}\" type=\"text/javascript\"></script>";
        public const string InlineScripts = "<script type=\"text/javascript\">{0}</script>";
        public const string InlineStyles = "<style type=\"text/css\">{0}</style>";
    }
}