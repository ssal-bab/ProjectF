using Cysharp.Threading.Tasks;
using H00N.DataTables;
using H00N.Extensions;
using H00N.OptOptions;
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
        [SerializeField] TMP_Text countText = null;
        [SerializeField] RectTransform sliderFillRect = null;
        
        [Space(10f)]
        [SerializeField] GameObject upgradeButtonObject = null;
        [SerializeField] GameObject upgradeCompleteButtonObject = null;
        [SerializeField] AddressableAsset<StorageUpgradePopupUI> upgradePopupUIPrefab = null;
        private StorageUpgradePopupUI upgradePopupUI = null;

        public new void Initialize()
        {
            base.Initialize();

            RefreshUI();
            upgradePopupUIPrefab.Initialize();
        }

        public new void Release()
        {
            base.Release();
            upgradePopupUI = null;
            StopAllCoroutines();
        }

        private void RefreshUI()
        {
            UserStorageData storageData = GameInstance.MainUser.storageData;
            GetStorageTableRow getStorageTableRow = new GetStorageTableRow(storageData.level);

            StorageTableRow tableRow = getStorageTableRow.currentStorageTableRow;
            if (tableRow == null)
            {
                Debug.LogError($"[StorageInfoPanelUI::RefreshUI] tableRow is null. CurrentLevel : {storageData.level}");
                return;
            }

            upgradeButtonObject.SetActive(!getStorageTableRow.isMaxLevel);
            upgradeCompleteButtonObject.SetActive(getStorageTableRow.isMaxLevel);

            storageIconImage.sprite = ResourceUtility.GetStorageIcon(tableRow.id);
            nameText.text = $"Lv.{tableRow.level} 적재소{tableRow.level}"; // 나중에 localizing 적용해야 함
            StartCoroutine(this.LoopRoutine(COUNT_UPDATE_DELAY, () => UpdateCountInfo(storageData, tableRow)));
        }

        private void UpdateCountInfo(UserStorageData storageData, StorageTableRow tableRow)
        {
            int usedCount = new GetStorageUsedCount(storageData).storageUsedCount;
            countText.text = $"{usedCount}/{tableRow.storeLimit}";
            
            Vector2 anchorMax = sliderFillRect.anchorMax;
            anchorMax.x = Mathf.Max(usedCount / (float)tableRow.storeLimit, 0);
            sliderFillRect.anchorMax = anchorMax;
        }

        public void OnTouchUpgradeButton()
        {
            int currentLevel = GameInstance.MainUser.storageData.level;
            GetStorageTableRow getStorageTableRow = new GetStorageTableRow(currentLevel);
            if(getStorageTableRow.isMaxLevel)
            {
                Debug.LogError($"[StorageInfoPanelUI::OnTouchUpgradeButton] Already max Level, but trying to open StorageUpgradePopupUI. CurrentLevel : {currentLevel}");
                return;
            }

            upgradePopupUI = PoolManager.Spawn<StorageUpgradePopupUI>(upgradePopupUIPrefab.Key, GameDefine.ContentsPopupFrame);
            upgradePopupUI.StretchRect();
            upgradePopupUI.Initialize(UpgradeStorage);
        }

        private async void UpgradeStorage()
        {
            StorageUpgradeResponse response = await NetworkManager.Instance.SendWebRequestAsync<StorageUpgradeResponse>(new StorageUpgradeRequest());
            if (response.result != ENetworkResult.Success)
                return;

            UserData mainUser = GameInstance.MainUser;
            mainUser.monetaData.gold -= response.usedGold;
            mainUser.storageData.materialStorage[response.usedCostItemID] -= response.usedCostItemCount;
            mainUser.storageData.level = response.currentLevel;

            if(upgradePopupUI != null)
                upgradePopupUI.OnTouchCloseButton();

            RefreshUI();
        }
    }
}
