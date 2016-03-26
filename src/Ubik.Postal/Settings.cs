using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ubik.Postal
{
    public class PostalSettings
    {
        public SmtpConfig Smtp { get; set; }
        public ImapConfig Imap { get; set; }
    }

}
