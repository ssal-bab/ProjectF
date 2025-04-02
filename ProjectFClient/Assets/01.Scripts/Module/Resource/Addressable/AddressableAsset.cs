using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace H00N.Resources
{
    [System.Serializable]
    public class AddressableAsset<T> where T : Object
    {
        private enum EState
        {
            Uninitialized,
            Initializing,
            Initialized
        }

        [SerializeField] private string key = null;
        public string Key => key;

        private T asset = null;
        public T Asset => asset;

        private EState state = EState.Uninitialized;
        public bool Initialized => state == EState.Initialized;

        public static implicit operator T(AddressableAsset<T> reference) => reference.Asset;
        // public static implicit operator Object(AddressableAsset<T> reference) => reference.Asset;

        public AddressableAsset()
        {
            state = EState.Uninitialized;
        }

        public async UniTask InitializeAsync()
        {
            if(Initialized)
                return;

            if(state == EState.Initializing)
            {
                await UniTask.WaitUntil(() => Initialized);
                return;
            }

            state = EState.Initializing;
            if (string.IsNullOrEmpty(key))
            {
                Debug.LogError("Key is null or empty.");
                state = EState.Initialized;
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

            state = EState.Initialized;
        }
    }
}
