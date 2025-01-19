using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace H00N.Resources.Pools
{
    public static class PoolManager
    {
        private static Transform poolParent = null;
        private static Dictionary<string, Pool> poolTable = null;

        private static bool initialized = false;
        public static bool Initialized => initialized;

        public static void Initialize(Transform parent)
        {
            poolParent = parent;
            poolTable = new Dictionary<string, Pool>();

            initialized = true;
        }

        public static void Release()
        {
            poolParent = null;
            foreach(Pool pool in poolTable.Values)
                pool.Release();
            poolTable = null;

            initialized = false;
        }

        public static PoolReference Spawn(string resourceName, Transform parent = null)
            => SpawnInternal(resourceName, parent, false).GetAwaiter().GetResult();

        public static async UniTask<PoolReference> SpawnAsync(string resourceName, Transform parent = null)
            => await SpawnInternal(resourceName, parent, true);

        private static async UniTask<PoolReference> SpawnInternal(string resourceName, Transform parent, bool isAsync)
        {
            if(poolTable.TryGetValue(resourceName, out Pool pool) == false)
            {
                ResourceHandle resourceHandle = isAsync ? await ResourceManager.LoadResourceHandleAsync(resourceName) : ResourceManager.LoadResourceHandle(resourceName);
                if(resourceHandle == null)
                {
                    Debug.LogWarning($"[Pool] Resource not found. : {resourceName}");
                    return null;
                }

                GameObject resource = resourceHandle.Get<GameObject>();
                PoolReference poolReference = resource.GetComponent<PoolReference>();
                if(poolReference == null)
                {
                    Debug.LogWarning($"[Pool] Current resource is not a PoolReference. : {resourceName}");
                    return null;
                }

                poolReference.InitializeResource(resourceHandle);

                pool = new Pool(poolReference);
                poolTable.Add(resourceName, pool);
            }

            if (isAsync)
                await UniTask.Yield();

            PoolReference instance = pool.Spawn();
            instance?.transform.SetParent(parent);
            return instance;
        }

        public static void Despawn(PoolReference instance)
            => DespawnInternal(instance, false).GetAwaiter().GetResult();

        public static async UniTask DespawnAsync(PoolReference instance)
            => await DespawnInternal(instance, true);

        private static async UniTask DespawnInternal(PoolReference instance, bool isAsync)
        {
            if(instance == null || instance.Handle == null)
                return;

            string resourceName = instance.Handle.ResourceName;
            if(poolTable.TryGetValue(resourceName, out Pool pool) == false)
            {
                Debug.LogWarning($"[Pool] Pool not found. : {resourceName}");
                Object.Destroy(instance.gameObject);
                return;
            }

            pool.Despawn(instance);

            if(isAsync)
                await UniTask.Yield();

            instance?.gameObject.SetActive(false);
            instance?.transform.SetParent(poolParent);
        }
    }
}