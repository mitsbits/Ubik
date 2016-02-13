using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Mvc.ViewFeatures.Internal;
using Ubik.Infra.Contracts;
using System.Linq;

namespace Ubik.Web.Basis.Contracts
{
    public interface IContentAccessor
    {
        IPageContent Current { get; }
    }

    public interface IServerFeedbackAccessor
    {
        IReadOnlyCollection<IServerResponse> Current { get; }
    }







    public interface IPageContent
    {
        string Title { get; }
        string Subtitle { get; }

        string[] Body { get; }
    }

    public class PageContent : IPageContent
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }

        public string[] Body { get; set; }
    }
}