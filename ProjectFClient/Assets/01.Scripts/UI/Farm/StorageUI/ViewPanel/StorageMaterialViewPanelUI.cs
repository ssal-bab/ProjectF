using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using H00N.Extensions;
using H00N.Resources;
using H00N.Resources.Pools;
using ProjectF.Datas;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectF.UI.Farms
{
    public class StorageMaterialViewPanelUI : StorageViewPanelUI
    {
        [SerializeField] ScrollRect scrollView = null;
        [SerializeField] AddressableAsset<StorageMaterialElementUI> elementPrefab = null;

        protected override void Awake()
        {
            base.Awake();
            elementPrefab.Initialize();
        }

        public override void Initialize(UserStorageData userStorageData, StorageUICallbackContainer callbackContainer)
        {
            base.Initialize(userStorageData, callbackContainer);
            if(userStorageData == null)
            {
                scrollView.content.DespawnAllChildren();
                return;
            }

            RefreshUIAsync(userStorageData.materialStorage);
        }

        private async void RefreshUIAsync(Dictionary<int, int> storageData)
        {
            scrollView.gameObject.SetActive(false);
            scrollView.content.DespawnAllChildren();

            foreach(var category in storageData)
                await AddToContainerAsync(category.Key, category.Value);

            scrollView.verticalNormalizedPosition = 1;
            scrollView.gameObject.SetActive(true);
        }

        private async UniTask AddToContainerAsync(int id, int count)
        {
            StorageMaterialElementUI ui = await PoolManager.SpawnAsync<StorageMaterialElementUI>(elementPrefab.Key);
            ui.transform.SetParent(scrollView.content);
            ui.Initialize(id, count);
        }
    }
}
