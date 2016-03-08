using System;

namespace Ubik.Web.Components.Contracts
{
    public interface IPartialViewComponent
    {
        string ClassName { get; }

        string TypeFullName { get; }

        Object[] InvokeParameters { get; }
    }
}