using System;
using H00N.DataTables;
using H00N.Resources.Pools;
using ProjectF.DataTables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectF.UI.Farms
{
    public class NestUpgradePopupUI : PoolableBehaviourUI
    {
        [SerializeField] Image currentIconImage = null;
        [SerializeField] TMP_Text currentLevelText = null;

        [Space(10f)]
        [SerializeField] Image nextIconImage = null;
        [SerializeField] TMP_Text nextLevelText = null;

        [Space(10f)]
        [SerializeField] UpgradeInfoUI eggInfoUI = null;
        [SerializeField] UpgradeInfoUI farmerInfoUI = null;

        [Space(10f)]
        [SerializeField] MaterialOptionUI materialOptionUI = null;
        [SerializeField] UpgradeButtonUI upgradeButtonUI = null;

        private Action<NestUpgradePopupUI> upgradeCallback = null;

        public void Initialize(Action<NestUpgradePopupUI> upgradeCallback)
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
            int currentLevel = GameInstance.MainUser.nestData.level;
            // GetFacilityTableRow<NestTable, NestTableRow> getFacilityTableRow = new GetFacilityTableRow<NestTable, NestTableRow>(currentLevel);
            NestLevelTableRow currentTableRow = DataTableManager.GetTable<NestLevelTable>().GetRowByLevel(currentLevel);
            NestLevelTableRow nextTableRow = DataTableManager.GetTable<NestLevelTable>().GetRowByLevel(currentLevel + 1);
            if (currentTableRow == null || nextTableRow == null)
                return;

            new SetSprite(currentIconImage, ResourceUtility.GetNestIconKey(currentTableRow.id));
            currentLevelText.text = $"Lv. {currentLevel}";

            new SetSprite(nextIconImage, ResourceUtility.GetNestIconKey(nextTableRow.id));
            nextLevelText.text = $"Lv. {currentLevel + 1}";

            // 로컬라이징 적용 해야한다.
            eggInfoUI.Initialize("알 저장 공간", $"{currentTableRow.eggStoreLimit}", $"{nextTableRow.eggStoreLimit}");
            farmerInfoUI.Initialize("최대 일꾼 수", $"{currentTableRow.farmerStoreLimit}", $"{nextTableRow.farmerStoreLimit}");

            // materialOptionUI.Initialize(currentTableRow.materialID, currentTableRow.materialCount);
            upgradeButtonUI.Initialize(currentTableRow.gold);
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