using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace H00N.Resources
{
    public class AddressableResourceLoader : IResourceLoader
    {
        public ResourceHandle LoadResource(string resourceName)
            => LoadResourceInternal(resourceName, false).GetAwaiter().GetResult();

        public async UniTask<ResourceHandle> LoadResourceAsync(string resourceName)
            => await LoadResourceInternal(resourceName, true);

        private async UniTask<ResourceHandle> LoadResourceInternal(string resourceName, bool isAsync)
        {
            AsyncOperationHandle<Object> requestHandle = Addressables.LoadAssetAsync<Object>(resourceName);

            if(isAsync)
                await requestHandle.Task;
            else
                requestHandle.WaitForCompletion();

            if(requestHandle.Status != AsyncOperationStatus.Succeeded)
            {
                Debug.LogWarning($"[Addressable] Failed to load resource. : {resourceName}");
                return null;
            }
            
            ResourceHandle resourceHandle = new ResourceHandle(resourceName, requestHandle.Result);
            Addressables.Release(requestHandle);

            return resourceHandle;
        }
    }
}