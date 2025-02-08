using Cysharp.Threading.Tasks;
using UnityEngine;

namespace H00N.Resources
{
    public interface IResourceLoader
    {
        public ResourceHandle LoadResource<T>(string resourceName) where T : Object;
        public UniTask<ResourceHandle> LoadResourceAsync<T>(string resourceName) where T : Object;
    }
}