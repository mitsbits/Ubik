using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Ubik.Domain.Core;

namespace Ubik.Web.Client.Backoffice
{
    public class ContentSetEvent : IEvent
    {


        public string Title { get; set; }

        public Payload Payload()
        {
            return new Payload() { PayloadType = GetType().AssemblyQualifiedName, PayloadJson = Newtonsoft.Json.JsonConvert.SerializeObject(this) };
        }
    }


}
