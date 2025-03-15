using System;
using H00N.DataTables;
using H00N.Resources.Pools;
using ProjectF.DataTables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static ProjectF.StringUtility;
using static ProjectF.GameDefine;

namespace ProjectF.UI.Farms
{
    public class StorageUpgradePopupUI : PoolableBehaviourUI
    {
        [SerializeField] Image currentIconImage = null;
        [SerializeField] TMP_Text currentLevelText = null;

        [Space(10f)]
        [SerializeField] Image nextIconImage = null;
        [SerializeField] TMP_Text nextLevelText = null;

        [Space(10f)]
        [SerializeField] UpgradeInfoUI capacityInfoUI = null;
        [SerializeField] UpgradeInfoUI sellGoldInfoUI = null;

        [Space(10f)]
        [SerializeField] MaterialOptionUI materialOptionUI = null;
        [SerializeField] UpgradeButtonUI upgradeButtonUI = null;

        private Action<StorageUpgradePopupUI> upgradeCallback = null;

        public void Initialize(Action<StorageUpgradePopupUI> upgradeCallback)
        {
            base.Initialize();

            this.upgradeCallback = upgradeCallback;
            RefreshUI();
        }

        protected override void Release()
        {
            base.Release();
            materialOptionUI.Release();
            upgradeButtonUI.Release();
        }

        public void RefreshUI()
        {
            int currentLevel = GameInstance.MainUser.storageData.level;
            GetFacilityTableRow<StorageTable, StorageTableRow> getFacilityTableRow = new GetFacilityTableRow<StorageTable, StorageTableRow>(currentLevel);
            StorageTableRow currentTableRow = getFacilityTableRow.currentTableRow;
            StorageTableRow nextTableRow = getFacilityTableRow.nextTableRow;
            if (currentTableRow == null)
                return;

            currentIconImage.sprite = ResourceUtility.GetStorageIcon(currentTableRow.id);
            currentLevelText.text = $"Lv. {currentLevel}";

            nextIconImage.sprite = ResourceUtility.GetStorageIcon(nextTableRow.id);
            nextLevelText.text = $"Lv. {currentLevel + 1}";

            // 로컬라이징 적용 해야한다.
            capacityInfoUI.Initialize("적재량", $"{currentTableRow.storeLimit}", $"{nextTableRow.storeLimit}");
            sellGoldInfoUI.Initialize("판매 이익", $"+{currentTableRow.priceMultiplier}%", $"+{nextTableRow.priceMultiplier}%");

            materialOptionUI.Initialize(currentTableRow.materialID, currentTableRow.materialCount);
            upgradeButtonUI.Initialize(currentTableRow.upgradeGold);
        }

        public void OnTouchCloseButton()
        {
            Release();
            PoolManager.DespawnAsync(this);
        }

        public void OnTouchUpgradeButton()
        {
            if (materialOptionUI.OptionChecked == false)
                return;

            if (upgradeButtonUI.UpgradePossible == false)
                return;

            upgradeCallback?.Invoke(this);
        }
    }
}