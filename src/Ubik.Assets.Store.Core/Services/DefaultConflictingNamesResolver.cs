using System;
using System.IO;
using System.Threading.Tasks;
using Ubik.Assets.Store.Core.Contracts;

namespace Ubik.Assets.Store.Core.Services
{
    public class DefaultConflictingNamesResolver : IConflictingNamesResolver
    {
        public Task<string> Resolve(string filename)
        {
            var name = Path.GetFileNameWithoutExtension(filename);
            var ext = Path.GetExtension(filename);
            return Task.FromResult(string.Format("{0}.{1}{2}", name, DateTime.UtcNow.Ticks, ext));
        }
    }
}