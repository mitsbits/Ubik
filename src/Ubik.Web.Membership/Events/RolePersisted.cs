using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ubik.Domain.Core;
using Ubik.Infra.DataManagement;

namespace Ubik.Web.Membership.Events
{
    public abstract class RoleEvent
    {
        private readonly int _id;
        private readonly string _name;

        protected RoleEvent(int id, string name)
        {
            _id = id;
            _name = name;
        }

        public int Id { get { return _id; } }
        public string Name { get { return _name; } }

        public Payload Payload()
        {
            return new Payload() { PayloadType = GetType().AssemblyQualifiedName, PayloadJson = Newtonsoft.Json.JsonConvert.SerializeObject(this) };
        }
    }

    public class RoleRowStateChanged : RoleEvent, IEvent, IPayload
    {
        private readonly RowState _state;

        public RoleRowStateChanged(int id, string name, RowState state):base(id, name)
        {
            _state = state;
        }


        public RowState State { get { return _state; } }

    }

}
