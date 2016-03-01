
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Ubik.Domain.Core;
using Ubik.Web.Client.Backoffice;
using Ubik.Web.Membership.Events;

namespace Ubik.Web.Client.Composition
{
    public class ContentSetEventHandler : IHandlesMessage<ContentSetEvent>
    {


        public ContentSetEventHandler() { }


        public Task Handle(ContentSetEvent message)
        {
            Debug.WriteLine("ContentSetEvent Title: {0}", message.Title);

            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri("http://localhost:5000/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var obj = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(message.Payload()), Encoding.UTF8, "application/json");
                var result = client.PostAsync("api/backoffice/events/receive/", obj).Result;
            }
            return Task.FromResult(0);
        }
       


    }


    public class ContentSetEventHandler2 : IHandlesMessage<ContentSetEvent>
    {


        public ContentSetEventHandler2() { }


        public Task Handle(ContentSetEvent message)
        {
            Debug.WriteLine("ContentSetEvent Title: {0} 2", message.Title);

            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri("http://localhost:5000/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var obj = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(message.Payload()), Encoding.UTF8, "application/json");
                var result = client.PostAsync("api/backoffice/events/receive/", obj).Result;
            }
            return Task.FromResult(0);
        }



    }

    public class RolePersistedHandler : IHandlesMessage<RolePersisted>
    {
        public RolePersistedHandler() { }

        public Task Handle(RolePersisted message)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5000/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var obj = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(message.Payload()), Encoding.UTF8, "application/json");
                var result = client.PostAsync("api/backoffice/events/receive/", obj).Result;
            }
            return Task.FromResult(0);
        }


    }
}
