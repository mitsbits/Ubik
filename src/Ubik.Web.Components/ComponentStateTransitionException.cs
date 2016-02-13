using System;
using Ubik.Web.Components.Exc;

namespace Ubik.Web.Components
{
    public class ComponentStateTransitionException : DesignByContractException
    {
        public ComponentStateTransitionException(string message)
            : base(message)
        {
        }

        public ComponentStateTransitionException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}