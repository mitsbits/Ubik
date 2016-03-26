using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ubik.Postal
{
    public class EmailFormatException : ApplicationException
    {
        public EmailFormatException(string email):base(string.Format("{0} is not well formated email.", email)){ }
    }
}
