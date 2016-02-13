using System;

namespace Ubik.Web.Components.Contracts
{
    internal interface IPartialViewComponent
    {
        string ClassName { get; }

        string TypeFullName { get; }

        Object[] InvokeParameters { get; }
    }
}