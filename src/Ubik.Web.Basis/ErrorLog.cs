using System;

namespace Ubik.Web.Basis
{
    public struct ErrorLog
    {
        public string Id { get; set; }
        public string Host { get; set; }
        public int ErrorCode { get; set; }
        public string ErrorType { get; set; }
        public string ErrorMessage { get; set; }
        public string User { get; set; }
        public DateTime ErrorDatetTimeUtc { get; set; }
    }
}