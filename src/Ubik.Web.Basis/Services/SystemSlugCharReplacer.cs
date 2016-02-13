using System.Linq;
using Ubik.Web.Basis.Contracts;

namespace Ubik.Web.Basis.Services
{
    public class SystemSlugCharReplacer : ISlugCharOmmiter
    {
        private static readonly char[] excludedChars =
        {
            ' '
            , ','
            , '.'
            , '/'
            , '\\'
            , '-'
            , '_'
            , '='
            , '«'
            , '»'
            , '~'
            ,'\''
            ,'"'
            ,'*'
            ,'+'
            ,';'
            ,'&'
            ,':'
            ,'¨'
            ,'…'
        };

        public bool Ommit(char source)
        {
            return excludedChars.Contains(source);
        }
    }
}