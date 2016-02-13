using Ubik.Web.Components.Contracts;

namespace Ubik.Web.Components.Ext
{
    internal static class ComponentExtensions
    {
        private static void GuardForDeleted(IComponent component)
        {
            var state = component.StateFlavor;
            if (state.HasFlag(ComponentStateFlavor.Deleted))
                throw new ComponentStateTransitionException("entity is deleted");
        }

        private static void GuardBeforeDelete(IComponent component)
        {
            var state = component.StateFlavor;
            if (state.HasFlag(ComponentStateFlavor.Published))
                throw new ComponentStateTransitionException("entity can not be deleted, it is published, unpublish first");
        }

        private static ComponentStateFlavor ApplyTransition(ComponentStateFlavor original, ComponentStateFlavor value,
            bool substract = false)
        {
            if (!substract)
            {
                if (!original.HasFlag(value)) original |= value;
            }
            else
            {
                if (original.HasFlag(value)) original &= ~value;
            }
            return original;
        }

        public static ComponentStateFlavor DoPublish(this IComponentCanPublishSuspend component)
        {
            GuardForDeleted(component);
            var state = component.StateFlavor;
            state = ApplyTransition(state, ComponentStateFlavor.Suspended, true);
            state = ApplyTransition(state, ComponentStateFlavor.Published);
            return state;
        }

        public static ComponentStateFlavor DoSuspend(this IComponentCanPublishSuspend component)
        {
            GuardForDeleted(component);
            var state = component.StateFlavor;
            state = ApplyTransition(state, ComponentStateFlavor.Published, true);
            state = ApplyTransition(state, ComponentStateFlavor.Suspended);
            return state;
        }

        public static bool GetIsPublished(this IComponentCanPublishSuspend component)
        {
            var state = component.StateFlavor;
            return !state.HasFlag(ComponentStateFlavor.Deleted) && state.HasFlag(ComponentStateFlavor.Published);
        }

        //public static ComponentStateFlavor DoCheckOut(this IComponentCanCheckOut component)
        //{
        //    GuardForDeleted(component);
        //    var state = component.StateFlavor;
        //    if (state.HasFlag(ComponentStateFlavor.CheckedOut))
        //        throw new ComponentStateTransitionException("entity is checked out already");
        //    state &= ApplyTransition(state, ComponentStateFlavor.CheckedOut, true);
        //    return state;
        //}

        //public static ComponentStateFlavor DoCheckIn(this IComponentCanCheckOut component)
        //{
        //    GuardForDeleted(component);
        //    var state = component.StateFlavor;
        //    if (!state.HasFlag(ComponentStateFlavor.CheckedOut))
        //        throw new ComponentStateTransitionException("entity has to be checked out first");
        //    state &= ApplyTransition(state, ComponentStateFlavor.CheckedOut, true);
        //    return state;
        //}

        //public static bool GetIsCheckedOut(this IComponentCanCheckOut component)
        //{
        //    var state = component.StateFlavor;
        //    return !state.HasFlag(ComponentStateFlavor.Deleted) && state.HasFlag(ComponentStateFlavor.CheckedOut);
        //}

        public static ComponentStateFlavor DoDelete(this IComponentCanBeDeleted component)
        {
            GuardBeforeDelete(component);
            const ComponentStateFlavor state = ComponentStateFlavor.Deleted;
            return state;
        }

        public static bool GetIsDeleted(this IComponentCanBeDeleted component)
        {
            var state = component.StateFlavor;
            return state.HasFlag(ComponentStateFlavor.Deleted);
        }
    }
}