using System;
using System.Collections.Generic;
using System.Linq;

using System.Threading.Tasks;

namespace Ubik.Postal.Contracts
{
    public interface IEmailService
    {
        Task SendSingleMail(MailAddress sender, MailAddress recipient, string subject, string body);
    }
}
