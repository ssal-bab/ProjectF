using System;
using H00N.DataTables;
using H00N.Resources.Pools;
using ProjectF.DataTables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        [SerializeField] TMP_Text capacityInfoText = null;
        [SerializeField] TMP_Text sellGoldInfoText = null;

        [Space(10f)]
        [SerializeField] MaterialOptionUI materialOptionUI = null;
        [SerializeField] UpgradeButtonUI upgradeButtonUI = null;

        private Action upgradeCallback = null;

        public void Initialize(Action upgradeCallback)
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
            StorageTableRow currentTableRow = DataTableManager.GetTable<StorageTable>().GetRowByLevel(currentLevel);
            StorageTableRow nextTableRow = DataTableManager.GetTable<StorageTable>().GetRowByLevel(currentLevel + 1);
            if (currentTableRow == null)
                return;

            currentIconImage.sprite = ResourceUtility.GetStorageIcon(currentTableRow.id);
            currentLevelText.text = $"Lv. {currentLevel}";

            nextIconImage.sprite = ResourceUtility.GetStorageIcon(nextTableRow.id);
            nextLevelText.text = $"Lv. {currentLevel + 1}";

            // 로컬라이징 적용 해야한다.
            
            capacityInfoText.text = $"<b>적재량</b> <b>{currentTableRow.storeLimit}</b> <color=black>></color> <color=#{GameDefine.DefaultGoldColor}><b>{nextTableRow.storeLimit}</b></color>";
            sellGoldInfoText.text = $"농작물 판매 골드 <color=#{GameDefine.DefaultGoldColor}>{nextTableRow.priceMultiplier - currentTableRow.priceMultiplier}%</color> 증가";

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

            upgradeCallback?.Invoke();
        }
    }
}