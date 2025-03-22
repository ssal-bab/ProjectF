using UnityEngine;

namespace H00N.Resources.Pools
{
    public static partial class PoolManager
    {
        public static T Spawn<T>(AddressableAsset<T> addressableAsset, Transform parent = null) where T : Component
        {
            return Spawn<T>(addressableAsset.Key);
        }
    }
}