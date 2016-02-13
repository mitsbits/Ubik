﻿using Microsoft.AspNet.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ubik.Web
{
    public static class HtmlHelperExtensions
    {
        public static void AddBackofficeBottom(this IHtmlHelper html, string urlstring)
        {
            const string prefix = @"/js/ubik/backoffice/";
            urlstring = urlstring.TrimStart('/');
            var path = string.Format("{0}{1}", prefix, (urlstring.EndsWith(".js") ? urlstring : urlstring + ".js"));
            html.Statics().FooterScripts.Add(path);
        }
    }
}
