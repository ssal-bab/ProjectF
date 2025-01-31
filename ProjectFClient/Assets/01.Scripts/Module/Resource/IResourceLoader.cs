using Cysharp.Threading.Tasks;
using UnityEngine;

namespace H00N.Resources
{
    public interface IResourceLoader
    {
        public ResourceHandle LoadResource(string resourceName);
        public UniTask<ResourceHandle> LoadResourceAsync(string resourceName);
    }
}