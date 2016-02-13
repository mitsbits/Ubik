using Ubik.EF6.Contracts;

namespace Ubik.Web.EF.Components
{
    public class PersistedTextual : ISequenceBase
    {
        public virtual int Id { get; set; }

        public virtual string Subject { get; set; }

        public virtual byte[] Summary { get; set; }

        public virtual byte[] Body { get; set; }
    }
}