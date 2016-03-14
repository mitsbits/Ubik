using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ubik.Assets.Store.Core.Contracts;
using Ubik.Domain.Core;

namespace Ubik.Assets.Store.Core.Events
{
    public class AssetCreatedEvent : IEvent
    {
        public AssetCreatedEvent(IAssetInfo asssetInfo) { AssetInfo = asssetInfo; }
        public IAssetInfo AssetInfo { get; private set; }
    }
}
