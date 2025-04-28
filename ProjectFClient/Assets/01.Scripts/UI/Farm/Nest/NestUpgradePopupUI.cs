using System;
using H00N.DataTables;
using H00N.Resources.Pools;
using ProjectF.DataTables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectF.UI.Farms
{
    public class NestUpgradePopupUI : UpgradePopupUI
    {
        [Space(10f)]
        [SerializeField] Image currentIconImage = null;
        [SerializeField] TMP_Text currentLevelText = null;

        [Space(10f)]
        [SerializeField] Image nextIconImage = null;
        [SerializeField] TMP_Text nextLevelText = null;

        [Space(10f)]
        [SerializeField] UpgradeInfoUI eggInfoUI = null;
        [SerializeField] UpgradeInfoUI farmerInfoUI = null;

        private Action<NestUpgradePopupUI> upgradeCallback = null;

        public async void Initialize(Action<NestUpgradePopupUI> upgradeCallback)
        {
            base.Initialize();
            await InitializeUpgradeUI();

            this.upgradeCallback = upgradeCallback;
            RefreshUI();
        }

        public void RefreshUI()
        {
            int currentLevel = GameInstance.MainUser.nestData.level;
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

            RefreshUpgradeUI(currentTableRow, DataTableManager.GetTable<NestUpgradeCostTable>().GetRowListByLevel(currentLevel));
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