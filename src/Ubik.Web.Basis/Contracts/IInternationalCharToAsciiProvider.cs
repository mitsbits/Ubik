using System.Collections.Generic;

namespace Ubik.Web.Basis.Contracts
{
    public interface IInternationalCharToAsciiProvider
    {
        IReadOnlyDictionary<char, char[]> Reference { get; }

        char[] Remap(char c);
    }
}