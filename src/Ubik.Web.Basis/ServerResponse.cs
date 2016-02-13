using System;
using Ubik.Infra;
using Ubik.Infra.Contracts;

namespace Ubik.Web.Basis
{
    public class ServerResponse : IServerResponse
    {
        public ServerResponseStatus Status { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }

        protected ServerResponse()
        {
        }

        public ServerResponse(ServerResponseStatus status, string title, string message)
        {
            Status = status;
            Title = title;
            Message = message;
        }

        public ServerResponse(Exception exc)
        {
            Status = ServerResponseStatus.ERROR;
            Title = exc.GetType().ToString();
            Message = exc.Message;
        }
    }
}