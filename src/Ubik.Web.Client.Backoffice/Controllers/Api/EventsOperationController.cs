using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ubik.Domain.Core;
using Ubik.Infra.Contracts;
using Ubik.Web.Membership.Events;


namespace Ubik.Web.Client.Backoffice.Controllers.Api
{
    public class EventsOperationController : BackofficeOperationsController
    {
        private readonly ICacheProvider _cache;

        public EventsOperationController(ICacheProvider cache)
        {
            _cache = cache;
        }

        [Route("api/backoffice/events/receive/")]
        [HttpPost]
        public async Task<IActionResult> Receive([FromBody]Payload message)
        {
            var type = Type.GetType(message.PayloadType);
            var @event = Newtonsoft.Json.JsonConvert.DeserializeObject(message.PayloadJson, type);

            if (type == typeof(RolePersisted)) await Handle(@event as RolePersisted);



       

            return Ok();
        }

        private Task Handle(RolePersisted @event)
        {
           
            _cache.RemoveItem(@event.CacheKey);
            return Task.FromResult(0);
        }

    }
}
