using System;

namespace Ubik.Web.Components.Exc
{
    public class ApplicationException : Exception
    {
        protected ApplicationException() : base() { }
        protected ApplicationException(string message) : base(message) { }
        protected ApplicationException(string message, Exception inner) : base(message, inner) { }
    }
}