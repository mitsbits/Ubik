using System;

namespace Ubik.Web.Client.Backoffice.ViewModel
{
    public class DeleteErrorLogViewModel
    {
        public Guid ErrorId { get; set; }
        public string RedirectUrl { get; set; }
    }

    public class DeleteErrorLogRangeViewModel
    {
        public Guid ErrorId { get; set; }
        public DateTime RangeStart { get; set; }
        public DateTime RangeEnd { get; set; }
        public string RedirectUrl { get; set; }
    }
}