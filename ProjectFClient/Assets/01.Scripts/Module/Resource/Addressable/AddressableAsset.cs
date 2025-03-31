using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace H00N.Resources
{
    [System.Serializable]
    public class AddressableAsset<T> where T : Object
    {
        [SerializeField] private string key = null;
        public string Key => key;

        private T asset = null;
        public T Asset => asset;

        public static implicit operator T(AddressableAsset<T> reference) => reference.Asset;
        // public static implicit operator Object(AddressableAsset<T> reference) => reference.Asset;

        public void Initialize()
        {
            if (string.IsNullOrEmpty(key))
            {
                Debug.LogError("Key is null or empty.");
                return;
            }

            // 컴포넌트면 GameObject를 로딩한 후에 GetComponent해준다.
            if(typeof(T).IsSubclassOf(typeof(Component)))
            {
                GameObject resource = ResourceManager.LoadResource<GameObject>(key);
                if(resource != null)
                    asset = resource.GetComponent<T>();
            }
            else
                asset = ResourceManager.LoadResource<T>(key);
        }

        public async UniTask InitializeAsync()
        {
            if (string.IsNullOrEmpty(key))
            {
                Debug.LogError("Key is null or empty.");
                return;
            }

            // 컴포넌트면 GameObject를 로딩한 후에 GetComponent해준다.
            if(typeof(T).IsSubclassOf(typeof(Component)))
            {
                GameObject resource = await ResourceManager.LoadResourceAsync<GameObject>(key);
                if(resource != null)
                    asset = resource.GetComponent<T>();
            }
            else
                asset = await ResourceManager.LoadResourceAsync<T>(key);
        }
    }
}
