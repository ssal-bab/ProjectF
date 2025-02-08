using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using H00N.Extensions;
using H00N.Resources;
using H00N.Resources.Pools;
using ProjectF.Datas;
using UnityEngine;

namespace ProjectF.UI.Farms
{
    public class CropStorageCropViewPanel : CropStorageViewPanel
    {
        [SerializeField] Transform containerTransform = null;
        [SerializeField] AddressableAsset<StorageCropElementUI> elementPrefab = null;

        [Header("Optoins")]
        [SerializeField] ToggleUI orderToggleUI = null; // true => asc / false => desc
        [SerializeField] ToggleUI filterToggleUI = null; // true => all / false => own
        
        protected override void Awake()
        {
            elementPrefab.Initialize();
        }

        public override void Initialize(UserCropStorageData userCropStorageData)
        {
            base.Initialize(userCropStorageData);

            // 토글 초기화
            orderToggleUI.SetToggle(true);
            filterToggleUI.SetToggle(true);

            if(userCropStorageData == null)
            {
                containerTransform.DespawnAllChildren();
                return;
            }
            
            RefreshUIAsync(userCropStorageData.cropStorage);
        }

        private async void RefreshUIAsync(Dictionary<int, Dictionary<int, int>> storageData)
        {
            containerTransform.DespawnAllChildren();

            foreach(var category in storageData)
            {
                foreach(var item in category.Value)
                    await AddToContainerAsync(category.Key, item.Key, item.Value);
            }
        }

        private async UniTask AddToContainerAsync(int id, int grade, int count)
        {
            if (filterToggleUI.ToggleValue == false && count <= 0)
                return;

            StorageCropElementUI ui = await PoolManager.SpawnAsync<StorageCropElementUI>(elementPrefab.Key);
            ui.transform.SetParent(containerTransform);
            if (orderToggleUI.ToggleValue == false)
                ui.transform.SetAsFirstSibling();

            ui.Initialize(id, grade, count);
        }
    }
}
