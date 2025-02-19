using System;
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
    public class StorageCropViewPanelUI : StorageViewPanelUI
    {
        [SerializeField] ScrollRect scrollView = null;
        [SerializeField] AddressableAsset<StorageCropElementUI> elementPrefab = null;

        [Header("Optoins")]
        [SerializeField] ToggleUI orderToggleUI = null; // true => asc / false => desc
        [SerializeField] ToggleUI filterToggleUI = null; // true => all / false => own

        private Dictionary<int, Dictionary<ECropGrade, int>> cropStorageData = null;
        private Action<int> sellCropCallback = null;
        
        protected override void Awake()
        {
            base.Awake();
            elementPrefab.Initialize();
        }

        public override void Initialize(UserStorageData userStorageData, StorageUICallbackContainer callbackContainer)
        {
            base.Initialize(userStorageData, callbackContainer);

            // 토글 초기화
            orderToggleUI.SetToggle(true);
            filterToggleUI.SetToggle(true);

            if(userStorageData == null)
            {
                scrollView.content.DespawnAllChildren();
                return;
            }
            
            cropStorageData = userStorageData.cropStorage;
            sellCropCallback = callbackContainer.SellCropCallback;
            RefreshUIAsync(cropStorageData, sellCropCallback);
        }

        public void RefreshSelf()
        {
            RefreshUIAsync(cropStorageData, sellCropCallback);
        }

        private async void RefreshUIAsync(Dictionary<int, Dictionary<ECropGrade, int>> storageData, Action<int> sellCropCallback)
        {
            scrollView.gameObject.SetActive(false);
            scrollView.content.DespawnAllChildren();
            foreach(var category in storageData)
            {
                foreach(var item in category.Value)
                    await AddToContainerAsync(category.Key, item.Key, item.Value, sellCropCallback);
            }

            scrollView.verticalNormalizedPosition = 1;
            scrollView.gameObject.SetActive(true);
        }

        private async UniTask AddToContainerAsync(int id, ECropGrade grade, int count, Action<int> sellCropCallback)
        {
            if (filterToggleUI.ToggleValue == false && count <= 0)
                return;

            StorageCropElementUI ui = await PoolManager.SpawnAsync<StorageCropElementUI>(elementPrefab.Key);
            ui.transform.SetParent(scrollView.content);
            if (orderToggleUI.ToggleValue == false)
                ui.transform.SetAsFirstSibling();

            ui.Initialize(id, grade, count, sellCropCallback);
        }
    }
}
