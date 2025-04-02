using System;
using H00N.Resources.Pools;
using ProjectF.Datas;
using ProjectF.DataTables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectF.UI.Farms
{
    public class FieldGroupUpgradePopupUI : PoolableBehaviourUI
    {
        [SerializeField] Image currentIconImage = null;
        [SerializeField] TMP_Text currentLevelText = null;

        [Space(10f)]
        [SerializeField] Image nextIconImage = null;
        [SerializeField] TMP_Text nextLevelText = null;

        [Space(10f)]
        [SerializeField] RarityUpgradeUI[] rarityUpgradeUIList = new RarityUpgradeUI[4];

        [Space(10f)]
        [SerializeField] MaterialOptionUI materialOptionUI = null;
        [SerializeField] UpgradeButtonUI upgradeButtonUI = null;

        private Action<FieldGroupUpgradePopupUI> upgradeCallback = null;
        private int fieldGroupID = 0;

        public void Initialize(Action<FieldGroupUpgradePopupUI> upgradeCallback, int fieldGroupID)
        {
            base.Initialize();

            this.upgradeCallback = upgradeCallback;
            this.fieldGroupID = fieldGroupID;
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
            int currentLevel = GameInstance.MainUser.fieldGroupData.fieldGroupDatas[fieldGroupID].level;
            GetFacilityTableRow<FieldGroupTable, FieldGroupTableRow> getFacilityTableRow = new GetFacilityTableRow<FieldGroupTable, FieldGroupTableRow>(currentLevel);
            FieldGroupTableRow currentTableRow = getFacilityTableRow.currentTableRow;
            FieldGroupTableRow nextTableRow = getFacilityTableRow.nextTableRow;
            if (currentTableRow == null || nextTableRow == null)
                return;

            new SetSprite(currentIconImage, ResourceUtility.GetFieldGroupIconKey(currentTableRow.id));
            currentLevelText.text = $"Lv. {currentLevel}";

            new SetSprite(nextIconImage, ResourceUtility.GetFieldGroupIconKey(nextTableRow.id));
            nextLevelText.text = $"Lv. {currentLevel + 1}";

            // 로컬라이징 적용 해야한다.
            for(int i = 0; i < rarityUpgradeUIList.Length; ++i)
            {
                float currentValue = currentTableRow.rateTable[i] / currentTableRow.totalRates * 100f;
                float nextValue = nextTableRow.rateTable[i] / nextTableRow.totalRates * 100f;
                rarityUpgradeUIList[i].Initialize((ECropGrade)i, currentValue, nextValue);
            }

            materialOptionUI.Initialize(currentTableRow.materialID, currentTableRow.materialCount);
            upgradeButtonUI.Initialize(currentTableRow.upgradeGold);
        }

        public void OnTouchCloseButton()
        {
            Release();
            PoolManager.Despawn(this);
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
