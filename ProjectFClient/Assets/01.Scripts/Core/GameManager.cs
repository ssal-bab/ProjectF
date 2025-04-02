using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using H00N.DataTables;
using H00N.Resources;
using H00N.Resources.Pools;
using Newtonsoft.Json;
using ProjectF.Networks;
using ProjectF.Quests;
using ProjectF.SubCharacters;
using UnityEngine;

namespace ProjectF
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager instance = null;
        public static GameManager Instance => instance;

        private RepeatQuestController repeatQuestController;

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
            UserActionObserver.Initialize();

            TextAsset dataTableJsonData = await ResourceManager.LoadResourceAsync<TextAsset>("DataTableJson");
            Dictionary<string, string> jsonDatas = JsonConvert.DeserializeObject<Dictionary<string, string>>(dataTableJsonData.text);
            DataTableManager.Initialize(jsonDatas);

            new FarmManager().Initialize();
            new NetworkManager().Initialize();
            new DialogueManager().Initialize();
            //new SubCharacterManager().Initialize();
        }

        public void OnLoginGameServer()
        {
            repeatQuestController = new RepeatQuestController();
            repeatQuestController.Initialize();
        }

        private void OnApplicationQuit()
        {
            DataTableManager.Release();
            PoolManager.Release();
            ResourceManager.Release();
            UserActionObserver.Release();

            FarmManager.Instance.Release();
            NetworkManager.Instance.Release();
            DialogueManager.Instance.Release();
            //SubCharacterManager.Instance.Release();
        }
    }
}
