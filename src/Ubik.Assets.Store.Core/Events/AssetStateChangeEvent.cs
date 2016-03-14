using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ubik.Domain.Core;

namespace Ubik.Assets.Store.Core.Events
{
    public class AssetStateChangeEvent<TKey>: IEvent
    {
        public AssetStateChangeEvent(TKey id, AssetState state) {
            Id = id;
            State = state;
        }

        public TKey Id { get; private set; }
        public AssetState State { get; private set; }
    }
}
