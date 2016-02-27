using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ubik.Web.Basis.Contracts
{
    public interface IPageContent
    {
        string Title { get; }
        string Subtitle { get; }

        string[] Body { get; }
    }


}
