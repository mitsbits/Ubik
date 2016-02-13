using Microsoft.AspNet.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using Ubik.Web.BuildingBlocks.Contracts;
using Ubik.Web.Components.Contracts;

namespace Ubik.Web.BuildingBlocks.Ext
{
    public static class Extensions
    {
        public static void RenderViewSection(this HtmlHelper html, ISection section, Action<Exception> handleException = null)
        {
            IEnumerable<ISlot> items =
                section.Slots.Where(x => x.SectionSlotInfo.Enabled)
                .OrderBy(x => x.SectionSlotInfo.Ordinal)
                .ToList();
            if (!items.Any()) return;
            foreach (var item in items)
            {
                try
                {
                    var toRender = item.Module as IHtmlHelperRendersMe;
                    toRender?.Render(html);
                }
                catch (Exception ex)
                {
                    handleException?.Invoke(ex);
                }
            }
        }

        public static int RoundOff(this int i, int round = 10)
        {
            return ((int)Math.Round(i / (double)round)) * round;
        }
    }
}