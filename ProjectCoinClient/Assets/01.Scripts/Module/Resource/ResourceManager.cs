using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace H00N.Resources
{
    public static class ResourceManager
    {
        private static IResourceLoader resourceLoader = null;
        private static Dictionary<string, ResourceHandle> resourceCache = null;

        private static bool initialized = false;
        public static bool Initialized => initialized;

        public static void Initialize(IResourceLoader loader)
        {
            resourceLoader = loader;
            resourceCache = new Dictionary<string, ResourceHandle>();

            initialized = true;
        }

        public static void Release()
        {
            resourceLoader = null;
            foreach(ResourceHandle handle in resourceCache.Values)
                handle?.Release();
            resourceCache = null;

            initialized = false;
        }

        public static async UniTask<ResourceHandle> LoadResourceHandleAsync(string resourceName)
            => await LoadResourceHandleInternal(resourceName, true);

        public static ResourceHandle LoadResourceHandle(string resourceName) 
            => LoadResourceHandleInternal(resourceName, false).GetAwaiter().GetResult();

        private static async UniTask<ResourceHandle> LoadResourceHandleInternal(string resourceName, bool isAsync)
        {
            if (resourceCache.TryGetValue(resourceName, out ResourceHandle resourceHandle) == false)
            {
                resourceHandle = isAsync ? await resourceLoader.LoadResourceAsync(resourceName) : resourceLoader.LoadResource(resourceName);
                if (resourceHandle == null)
                    Debug.LogWarning($"[Resource] Resource not found. : {resourceName}");

                resourceCache.Add(resourceName, resourceHandle);
            }

            return resourceHandle;
        }

        public static async UniTask<T> LoadResourceAsync<T>(string resourceName) where T : Object
            => await LoadResourceInternal<T>(resourceName, false);

        public static T LoadResource<T>(string resourceName) where T : Object
            => LoadResourceInternal<T>(resourceName, false).GetAwaiter().GetResult();

        private static async UniTask<T> LoadResourceInternal<T>(string resourceName, bool isAsync) where T : Object
        {
            ResourceHandle resourceHandle = isAsync ? await LoadResourceHandleAsync(resourceName) : LoadResourceHandle(resourceName);
            if(resourceHandle == null)
                return null;

            T resource = resourceHandle.Get<T>();
            if (resource == null)
            {
                Debug.LogWarning($"[Resource] Resource type does not match. : {typeof(T).Name}");
                return null;
            }

            return resource;
        }

        public static void ReleaseResource(string resourceName)
        {
            if(resourceCache.TryGetValue(resourceName, out ResourceHandle handle) == false)
                return;

            if(handle == null)
                return;

            handle.Release();
        }

        public static async UniTask<T> LoadResourceWithoutCahingAsync<T>(string resourceName) where T : Object
            => await LoadResourceWithoutCachingInternal<T>(resourceName, true);

        public static T LoadResourceWithoutCaching<T>(string resourceName) where T : Object
            => LoadResourceWithoutCachingInternal<T>(resourceName, false).GetAwaiter().GetResult();

        public static async UniTask<T> LoadResourceWithoutCachingInternal<T>(string resourceName, bool isAsync) where T : Object
        {
            ResourceHandle handle = isAsync ? await resourceLoader.LoadResourceAsync(resourceName) : resourceLoader.LoadResource(resourceName);
            if(handle == null)
            {
                Debug.LogWarning($"[Resource] Resource not found. : {resourceName}");
                return null;
            }

            T resource = handle.Get<T>();

            handle.Release();
            return resource;
        }
    }
}