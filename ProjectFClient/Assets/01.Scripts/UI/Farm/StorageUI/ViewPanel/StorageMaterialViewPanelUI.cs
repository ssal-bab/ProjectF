using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using H00N.Extensions;
using H00N.Resources;
using H00N.Resources.Pools;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectF.UI.Farms
{
    public class StorageMaterialViewPanelUI : StorageViewPanelUI
    {
        [SerializeField] ScrollRect scrollView = null;
        [SerializeField] AddressableAsset<StorageMaterialElementUI> elementPrefab = null;

        public override async void Initialize()
        {
            base.Initialize();

            await elementPrefab.InitializeAsync();
            RefreshUI(GameInstance.MainUser.storageData.materialStorage);
        }

        private void RefreshUI(Dictionary<int, int> storageData)
        {
            scrollView.gameObject.SetActive(false);
            scrollView.content.DespawnAllChildren();

            foreach(var category in storageData)
                AddToContainer(category.Key, category.Value);

            scrollView.verticalNormalizedPosition = 1;
            scrollView.gameObject.SetActive(true);
        }

        private void AddToContainer(int id, int count)
        {
            StorageMaterialElementUI ui = PoolManager.Spawn<StorageMaterialElementUI>(elementPrefab, scrollView.content);
            ui.InitializeTransform();
            ui.Initialize(id, count);
        }
    }
}
