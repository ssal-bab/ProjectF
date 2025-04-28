using System;
using H00N.DataTables;
using H00N.Resources.Pools;
using ProjectF.DataTables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectF.UI.Storages
{
    public class StorageUpgradePopupUI : UpgradePopupUI
    {
        [Space(10f)]
        [SerializeField] Image currentIconImage = null;
        [SerializeField] TMP_Text currentLevelText = null;

        [Space(10f)]
        [SerializeField] Image nextIconImage = null;
        [SerializeField] TMP_Text nextLevelText = null;

        [Space(10f)]
        [SerializeField] UpgradeInfoUI capacityInfoUI = null;
        [SerializeField] UpgradeInfoUI sellGoldInfoUI = null;

        private Action<StorageUpgradePopupUI> upgradeCallback = null;

        public async void Initialize(Action<StorageUpgradePopupUI> upgradeCallback)
        {
            base.Initialize();
            await InitializeUpgradeUI();

            this.upgradeCallback = upgradeCallback;
            RefreshUI();
        }

        public void RefreshUI()
        {
            int currentLevel = GameInstance.MainUser.storageData.level;
            StorageLevelTableRow currentTableRow = DataTableManager.GetTable<StorageLevelTable>().GetRowByLevel(currentLevel);
            StorageLevelTableRow nextTableRow = DataTableManager.GetTable<StorageLevelTable>().GetRowByLevel(currentLevel + 1);
            if (currentTableRow == null)
                return;

            new SetSprite(currentIconImage, ResourceUtility.GetStorageIconKey(currentTableRow.id));
            currentLevelText.text = $"Lv. {currentLevel}";

            new SetSprite(nextIconImage, ResourceUtility.GetStorageIconKey(nextTableRow.id));
            nextLevelText.text = $"Lv. {currentLevel + 1}";

            // 로컬라이징 적용 해야한다.
            capacityInfoUI.Initialize("적재량", $"{currentTableRow.storeLimit}", $"{nextTableRow.storeLimit}");
            sellGoldInfoUI.Initialize("판매 이익", $"+{currentTableRow.priceMultiplier}%", $"+{nextTableRow.priceMultiplier}%");

            // materialOptionUI.Initialize(currentTableRow.materialID, currentTableRow.materialCount);
            RefreshUpgradeUI(currentTableRow, DataTableManager.GetTable<StorageUpgradeCostTable>().GetRowListByLevel(currentLevel));
        }

        public void OnTouchCloseButton()
        {
            Release();
            PoolManager.Despawn(this);
        }

        public void OnTouchUpgradeButton()
        {
            if (GetUpgradePossible() == false)
                return;

            upgradeCallback?.Invoke(this);
        }
    }
}