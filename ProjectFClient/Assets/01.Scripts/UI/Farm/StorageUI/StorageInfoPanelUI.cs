using H00N.Extensions;
using H00N.Resources;
using H00N.Resources.Pools;
using ProjectF.Datas;
using ProjectF.DataTables;
using ProjectF.Networks;
using ProjectF.Networks.Packets;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectF.UI.Farms
{
    public class StorageInfoPanelUI : MonoBehaviourUI
    {
        private const float COUNT_UPDATE_DELAY = 10f;

        [SerializeField] Image storageIconImage = null;
        [SerializeField] TMP_Text nameText = null;
        [SerializeField] SliderUI sliderUI = null;
        
        [Space(10f)]
        [SerializeField] GameObject upgradeButtonObject = null;
        [SerializeField] GameObject upgradeCompleteButtonObject = null;
        [SerializeField] AddressableAsset<StorageUpgradePopupUI> upgradePopupUIPrefab = null;

        public new void Initialize()
        {
            base.Initialize();

            RefreshUI();
            upgradePopupUIPrefab.Initialize();
        }

        public new void Release()
        {
            base.Release();
            StopAllCoroutines();
        }

        private void RefreshUI()
        {
            UserStorageData storageData = GameInstance.MainUser.storageData;
            GetFacilityTableRow<StorageTable, StorageTableRow> getFacilityTableRow = new GetFacilityTableRow<StorageTable, StorageTableRow>(storageData.level);
            
            StorageTableRow tableRow = getFacilityTableRow.currentTableRow;
            if (tableRow == null)
            {
                Debug.LogError($"[StorageInfoPanelUI::RefreshUI] tableRow is null. CurrentLevel : {storageData.level}");
                return;
            }

            upgradeButtonObject.SetActive(!getFacilityTableRow.isMaxLevel);
            upgradeCompleteButtonObject.SetActive(getFacilityTableRow.isMaxLevel);

            storageIconImage.sprite = ResourceUtility.GetStorageIcon(tableRow.id);
            nameText.text = $"Lv.{tableRow.level} 적재소{tableRow.level}"; // 나중에 localizing 적용해야 함
            StartCoroutine(this.LoopRoutine(COUNT_UPDATE_DELAY, () => UpdateCountInfo(storageData, tableRow)));
        }

        private void UpdateCountInfo(UserStorageData storageData, StorageTableRow tableRow)
        {
            int usedCount = new GetStorageUsedCount(storageData).storageUsedCount;
            sliderUI.RefreshUI(tableRow.storeLimit, usedCount);
        }

        public void OnTouchUpgradeButton()
        {
            int currentLevel = GameInstance.MainUser.storageData.level;
            GetFacilityTableRow<StorageTable, StorageTableRow> getFacilityTableRow = new GetFacilityTableRow<StorageTable, StorageTableRow>(currentLevel);
            if(getFacilityTableRow.isMaxLevel)
            {
                Debug.LogError($"[StorageInfoPanelUI::OnTouchUpgradeButton] Already max Level, but trying to open StorageUpgradePopupUI. CurrentLevel : {currentLevel}");
                return;
            }

            StorageUpgradePopupUI upgradePopupUI = PoolManager.Spawn<StorageUpgradePopupUI>(upgradePopupUIPrefab.Key, GameDefine.ContentsPopupFrame);
            upgradePopupUI.StretchRect();
            upgradePopupUI.Initialize(UpgradeStorage);
        }

        private async void UpgradeStorage(StorageUpgradePopupUI ui)
        {
            StorageUpgradeResponse response = await NetworkManager.Instance.SendWebRequestAsync<StorageUpgradeResponse>(new StorageUpgradeRequest());
            if (response.result != ENetworkResult.Success)
                return;

            UserData mainUser = GameInstance.MainUser;
            mainUser.monetaData.gold -= response.usedGold;
            mainUser.storageData.materialStorage[response.usedCostItemID] -= response.usedCostItemCount;
            mainUser.storageData.level = response.currentLevel;
            mainUser.storageData.OnLevelChangedEvent?.Invoke(mainUser.storageData.level);

            if(ui != null)
                ui.OnTouchCloseButton();

            RefreshUI();
        }
    }
}
