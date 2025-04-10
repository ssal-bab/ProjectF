using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace H00N.Resources
{
    public class AddressableResourceLoader : IResourceLoader
    {
        public async UniTask<ResourceHandle> LoadResourceAsync<T>(string resourceName) where T : Object
            => await LoadResourceInternal<T>(resourceName, true);

        private async UniTask<ResourceHandle> LoadResourceInternal<T>(string resourceName, bool isAsync) where T : Object
        {
            try {
                AsyncOperationHandle<T> requestHandle = Addressables.LoadAssetAsync<T>(resourceName);

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
                return resourceHandle;
            } 
            catch(Exception err) {
                Debug.LogWarning(err);
                return null;
            }
        }
    }
}