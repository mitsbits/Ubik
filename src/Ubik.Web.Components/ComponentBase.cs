using Ubik.Web.Components.Contracts;
using Ubik.Web.Components.Ext;

namespace Ubik.Web.Components
{
    public class ComponentBase<TKey> : EntityBase<TKey>, IComponent, IComponentCanPublishSuspend, IComponentCanBeDeleted
    {
        #region IComponent

        protected ComponentBase(TKey id)
        {
            Id = id;
            StateFlavor = ComponentStateFlavor.Empty;
        }

        protected ComponentBase()
        {
            Id = default(TKey);
            StateFlavor = ComponentStateFlavor.Empty;
        }

        public ComponentStateFlavor StateFlavor { get; private set; }

        public void SetState(ComponentStateFlavor flavor)
        {
            StateFlavor = flavor;
        }

        #endregion IComponent

        #region IComponentCanPublishSuspend

        public void Publish()
        {
            this.DoPublish();
        }

        public void Suspend()
        {
            this.DoSuspend();
        }

        public bool IsPublished
        {
            get { return this.GetIsPublished(); }
        }

        #endregion IComponentCanPublishSuspend

        #region IComponentCanBeDeleted

        public void Delete()
        {
            this.DoDelete();
        }

        public bool IsDeleted
        {
            get { return this.GetIsDeleted(); }
        }

        #endregion IComponentCanBeDeleted
    }
}