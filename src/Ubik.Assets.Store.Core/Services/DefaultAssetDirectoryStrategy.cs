using System;
using Ubik.Assets.Store.Core.Contracts;

namespace Ubik.Assets.Store.Core.Services
{
    public class DefaultAssetDirectoryStrategy : IAssetDirectoryStrategy<int>
    {
        private readonly Func<IAssetInfo<int>, string> _deleg = (a) =>
        {
            var key = a.Id;
            var outer = key.RoundOff(500) + 500;
            var inner = key.RoundOff(50) + 50;
            return string.Format(@"{0}/{1}", outer, inner);
        };

        public DefaultAssetDirectoryStrategy()
        {
        }

        public DefaultAssetDirectoryStrategy(Func<IAssetInfo<int>, string> deleg) : this()
        {
            _deleg = deleg;
        }

        public string ParentFolder(IAssetInfo<int> asset)
        {
            return _deleg.Invoke(asset);
        }
    }

    internal static class Ext
    {
        public static int RoundOff(this int i, int round = 10)
        {
            return (int)Math.Floor(i / (double)round) * round;
            //return ((int)Math.Round(i / (double)round)) * round;
        }
    }
}