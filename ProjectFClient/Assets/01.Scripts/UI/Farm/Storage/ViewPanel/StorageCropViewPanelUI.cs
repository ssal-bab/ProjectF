using System;
using Cysharp.Threading.Tasks;
using H00N.DataTables;
using H00N.Extensions;
using H00N.Resources;
using H00N.Resources.Pools;
using ProjectF.Datas;
using ProjectF.DataTables;
using ProjectF.Networks;
using ProjectF.Networks.Packets;
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

        public override void Initialize()
        {
            base.Initialize();

            // 토글 초기화
            orderToggleUI.SetToggle(true);
            filterToggleUI.SetToggle(true);

            RefreshUIAsync();
        }

        public void RefreshSelf()
        {
            RefreshUIAsync();
        }

        private async void RefreshUIAsync()
        {
            scrollView.gameObject.SetActive(false);
            scrollView.content.DespawnAllChildren();

            await elementPrefab.InitializeAsync();

            UserStorageData storageData = GameInstance.MainUser.storageData;
            CropTable cropTable = DataTableManager.GetTable<CropTable>();
            foreach(var cropTableRow in cropTable)
            {
                if(storageData.cropStorage.TryGetValue(cropTableRow.id, out var category) == false)
                    continue;

                foreach(ECropGrade cropGrade in EnumHelper.GetValues<ECropGrade>())
                    AddToContainerAsync(cropTableRow.id, cropGrade, category[cropGrade]);
            }

            scrollView.verticalNormalizedPosition = 1;
            scrollView.gameObject.SetActive(true);
        }

        private void AddToContainerAsync(int id, ECropGrade grade, int count)
        {
            if (filterToggleUI.ToggleValue == false && count <= 0)
                return;

            StorageCropElementUI ui = PoolManager.Spawn<StorageCropElementUI>(elementPrefab, scrollView.content);
            ui.InitializeTransform();
            if (orderToggleUI.ToggleValue == false)
                ui.transform.SetAsFirstSibling();

            ui.Initialize(id, grade, count, SellCrop);
            //ui.StretchRect();
        }

        private async void SellCrop(StorageCropElementUI ui, int id, ECropGrade grade)
        {
            SellCropResponse response = await NetworkManager.Instance.SendWebRequestAsync<SellCropResponse>(new SellCropRequest(id, grade));
            if(response.result != ENetworkResult.Success)
                return;

            UserData mainUser = GameInstance.MainUser;
            mainUser.storageData.cropStorage[id][grade] -= response.soldCropCount;
            mainUser.monetaData.gold += response.earnedGold;
            ui.Initialize(id, grade, mainUser.storageData.cropStorage[id][grade], SellCrop);
        }
    }
}
