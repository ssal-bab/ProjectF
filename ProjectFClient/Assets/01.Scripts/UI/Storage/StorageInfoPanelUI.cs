using Cysharp.Threading.Tasks;
using H00N.DataTables;
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

namespace ProjectF.UI.Storages
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
            upgradePopupUIPrefab.InitializeAsync().Forget();
        }

        public new void Release()
        {
            base.Release();
            StopAllCoroutines();
        }

        private void RefreshUI()
        {
            UserStorageData storageData = GameInstance.MainUser.storageData;
            // GetFacilityTableRow<StorageTable, StorageTableRow> getFacilityTableRow = new GetFacilityTableRow<StorageTable, StorageTableRow>(storageData.level);
            
            StorageLevelTableRow tableRow = DataTableManager.GetTable<StorageLevelTable>().GetRowByLevel(storageData.level + 1);
            if (tableRow == null)
            {
                Debug.LogError($"[StorageInfoPanelUI::RefreshUI] tableRow is null. CurrentLevel : {storageData.level}");
                return;
            }

            StorageLevelTableRow nextLevelTableRow = DataTableManager.GetTable<StorageLevelTable>().GetRowByLevel(storageData.level + 1);
            bool isMaxLevel = nextLevelTableRow == null;
            upgradeButtonObject.SetActive(!isMaxLevel);
            upgradeCompleteButtonObject.SetActive(isMaxLevel);

            new SetSprite(storageIconImage, ResourceUtility.GetStorageIconKey(tableRow.id));
            nameText.text = $"Lv.{tableRow.level} 적재소{tableRow.level}"; // 나중에 localizing 적용해야 함
            StartCoroutine(this.LoopRoutine(COUNT_UPDATE_DELAY, () => UpdateCountInfo(storageData, tableRow)));
        }

        private void UpdateCountInfo(UserStorageData storageData, StorageLevelTableRow tableRow)
        {
            int usedCount = new GetStorageUsedCount(storageData).storageUsedCount;
            sliderUI.RefreshUI(tableRow.storeLimit, usedCount);
        }

        public async void OnTouchUpgradeButton()
        {
            await upgradePopupUIPrefab.InitializeAsync();

            int currentLevel = GameInstance.MainUser.storageData.level;

            StorageLevelTableRow nextLevelTableRow = DataTableManager.GetTable<StorageLevelTable>().GetRowByLevel(currentLevel + 1);
            if(nextLevelTableRow == null)
            {
                Debug.LogError($"[StorageInfoPanelUI::OnTouchUpgradeButton] Already max Level, but trying to open StorageUpgradePopupUI. CurrentLevel : {currentLevel}");
                return;
            }

            StorageUpgradePopupUI upgradePopupUI = PoolManager.Spawn<StorageUpgradePopupUI>(upgradePopupUIPrefab, GameDefine.ContentPopupFrame);
            upgradePopupUI.StretchRect();
            upgradePopupUI.Initialize(UpgradeStorage);
        }

        private async void UpgradeStorage(StorageUpgradePopupUI ui)
        {
            StorageUpgradeResponse response = await NetworkManager.Instance.SendWebRequestAsync<StorageUpgradeResponse>(new StorageUpgradeRequest());
            if (response.result != ENetworkResult.Success)
                return;

            UserData mainUser = GameInstance.MainUser;

            StorageLevelTableRow tableRow = DataTableManager.GetTable<StorageLevelTable>().GetRowByLevel(response.currentLevel - 1);
            mainUser.monetaData.gold -= tableRow.gold;

            new ApplyUpgradeCost<StorageUpgradeCostTableRow>(mainUser.storageData, DataTableManager.GetTable<StorageUpgradeCostTable>().GetRowListByLevel(response.currentLevel - 1));

            mainUser.storageData.level = response.currentLevel;
            mainUser.storageData.OnLevelChangedEvent?.Invoke(mainUser.storageData.level);

            if(ui != null)
                ui.OnTouchCloseButton();

            RefreshUI();
        }
    }
}
