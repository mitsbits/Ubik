using System.Text.RegularExpressions;

namespace Ubik.Postal
{
    public static class Extensions
    {
        public static bool WellFormedMail(string email)
        {
            Regex g = new Regex(@"^(([^<>()[\]\\.,;:\s@\""]+"
                                + @"(\.[^<>()[\]\\.,;:\s@\""]+)*)|(\"".+\""))@"
                                + @"((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}"
                                + @"\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+"
                                + @"[a-zA-Z]{2,}))$");
            return g.Match(email).Success;
        }
    }
}