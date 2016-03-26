using System.Text.RegularExpressions;
using Ubik.Postal.Services;

namespace Ubik.Postal
{
    public static class Extensions
    {
        public static bool WellFormedMail(this string email)
        {
            Regex g = new Regex(@"^(([^<>()[\]\\.,;:\s@\""]+"
                                + @"(\.[^<>()[\]\\.,;:\s@\""]+)*)|(\"".+\""))@"
                                + @"((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}"
                                + @"\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+"
                                + @"[a-zA-Z]{2,}))$");
            return g.Match(email).Success;
        }

        public static bool ValidEmail(this string email, bool  allowInternational = false)
        {
            return EmailValidator.Validate(email, allowInternational);
        }
    }
}