using System;
using Ubik.Web.Basis.Contracts;
using Ubik.Web.Client.Backoffice.Contracts;

namespace Ubik.Web.Client.Backoffice
{
    internal class BackofficeContent : IBackofficeContent, IPageContent
    {
        public string Title { get; set; }

        public string Subtitle { get; set; }

        public string[] Body
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}