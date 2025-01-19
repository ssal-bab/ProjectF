using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using H00N.DataTables;
using H00N.Resources;
using H00N.Resources.Pools;
using Newtonsoft.Json;
using ProjectCoin.Networks;
using UnityEngine;

namespace ProjectCoin
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager instance = null;
        public static GameManager Instance => instance;

        private void Awake()
        {
            if(instance != null)
                DestroyImmediate(instance.gameObject);

            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public async UniTask InitializeAsync()
        {
            ResourceManager.Initialize(new AddressableResourceLoader());
            PoolManager.Initialize(transform);

            TextAsset dataTableJsonData = await ResourceManager.LoadResourceAsync<TextAsset>("DataTableJson");
            Dictionary<string, string> jsonDatas = JsonConvert.DeserializeObject<Dictionary<string, string>>(dataTableJsonData.text);
            DataTableManager.Initialize(jsonDatas);

            // GetComponent<FarmManager>().Initialize();
            new NetworkManager().Initialize();
        }

        private void OnApplicationQuit()
        {
            DataTableManager.Release();
            PoolManager.Release();
            ResourceManager.Release();

            // FarmManager.Instance.Release();
            NetworkManager.Instance.Release();
        }
    }
}
