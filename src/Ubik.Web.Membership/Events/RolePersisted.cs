using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ubik.Domain.Core;

namespace Ubik.Web.Membership.Events
{
    public class RolePersisted :IEvent, IPayload
    {
        private readonly int _id;
        private readonly string _name;

        public int Id { get { return _id; } }
        public string Name { get { return _name; } }
        public RolePersisted(int id, string name)
        {
            _id = id;
            _name = name;
        }
        public Payload Payload()
        {
            return new Payload() { PayloadType = GetType().AssemblyQualifiedName, PayloadJson = Newtonsoft.Json.JsonConvert.SerializeObject(this) };
        }
    }
}
