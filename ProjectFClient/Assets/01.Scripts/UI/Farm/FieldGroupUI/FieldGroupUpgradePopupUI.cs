using System;
using H00N.DataTables;
using H00N.Resources.Pools;
using ProjectF.DataTables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectF.UI.Farms
{
    public class FieldGroupUpgradePopupUI : UpgradePopupUI
    {
        [Space(10f)]
        [SerializeField] Image currentIconImage = null;
        [SerializeField] TMP_Text currentLevelText = null;

        [Space(10f)]
        [SerializeField] Image nextIconImage = null;
        [SerializeField] TMP_Text nextLevelText = null;

        [Space(10f)]
        [SerializeField] RarityUpgradeUI[] rarityUpgradeUIList = new RarityUpgradeUI[4];

        private Action<FieldGroupUpgradePopupUI> upgradeCallback = null;
        private int fieldGroupID = 0;

        public async void Initialize(Action<FieldGroupUpgradePopupUI> upgradeCallback, int fieldGroupID)
        {
            base.Initialize();
            await InitializeUpgradeUI();

            this.upgradeCallback = upgradeCallback;
            this.fieldGroupID = fieldGroupID;
            RefreshUI();
        }

        public void RefreshUI()
        {
            int currentLevel = GameInstance.MainUser.fieldGroupData.fieldGroupDatas[fieldGroupID].level;
            FieldGroupLevelTableRow currentTableRow = DataTableManager.GetTable<FieldGroupLevelTable>().GetRowByLevel(currentLevel);
            FieldGroupLevelTableRow nextTableRow = DataTableManager.GetTable<FieldGroupLevelTable>().GetRowByLevel(currentLevel + 1);
            if (currentTableRow == null || nextTableRow == null)
                return;

            new SetSprite(currentIconImage, ResourceUtility.GetFieldGroupIconKey(currentTableRow.id));
            currentLevelText.text = $"Lv. {currentLevel}";

            new SetSprite(nextIconImage, ResourceUtility.GetFieldGroupIconKey(nextTableRow.id));
            nextLevelText.text = $"Lv. {currentLevel + 1}";

            // 로컬라이징 적용 해야한다.
            // for(int i = 0; i < rarityUpgradeUIList.Length; ++i)
            // {
            //     float currentValue = currentTableRow.rateTable[i] / currentTableRow.totalRates * 100f;
            //     float nextValue = nextTableRow.rateTable[i] / nextTableRow.totalRates * 100f;
            //     rarityUpgradeUIList[i].Initialize((ECropGrade)i, currentValue, nextValue);
            // }

            RefreshUpgradeUI(currentTableRow, DataTableManager.GetTable<FieldGroupUpgradeCostTable>().GetRowListByLevel(currentLevel));
        }

        public void OnTouchCloseButton()
        {
            Release();
            PoolManager.Despawn(this);
        }

        public void OnTouchUpgradeButton()
        {
            if (GetUpgradePossible())
                return;

            upgradeCallback?.Invoke(this);
        }
    }
}
