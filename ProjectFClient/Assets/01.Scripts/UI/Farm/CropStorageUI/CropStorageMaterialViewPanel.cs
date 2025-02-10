using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using H00N.Extensions;
using H00N.Resources;
using H00N.Resources.Pools;
using ProjectF.Datas;
using UnityEngine;

namespace ProjectF.UI.Farms
{
    public class CropStorageMaterialViewPanel : CropStorageViewPanel
    {
        [SerializeField] Transform containerTransform = null;
        [SerializeField] AddressableAsset<StorageMaterialElementUI> elementPrefab = null;

        protected override void Awake()
        {
            base.Awake();
            elementPrefab.Initialize();
        }

        public override void Initialize(UserCropStorageData userCropStorageData, CropStorageUICallbackContainer callbackContainer)
        {
            base.Initialize(userCropStorageData, callbackContainer);
            if(userCropStorageData == null)
            {
                containerTransform.DespawnAllChildren();
                return;
            }

            RefreshUIAsync(userCropStorageData.materialStorage);
        }

        private async void RefreshUIAsync(Dictionary<int, int> storageData)
        {
            containerTransform.DespawnAllChildren();

            foreach(var category in storageData)
                await AddToContainerAsync(category.Key, category.Value);
        }

        private async UniTask AddToContainerAsync(int id, int count)
        {
            StorageMaterialElementUI ui = await PoolManager.SpawnAsync<StorageMaterialElementUI>(elementPrefab.Key);
            ui.transform.SetParent(containerTransform);
            ui.Initialize(id, count);
        }
    }
}
