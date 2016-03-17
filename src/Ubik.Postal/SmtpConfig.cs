using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ubik.Postal
{
    public class SmtpConfig
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string UseName { get; set; }
        public string Password { get; set; }
        public bool SSL { get; set; }
    }
}
