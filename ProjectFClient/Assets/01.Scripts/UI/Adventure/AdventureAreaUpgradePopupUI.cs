using System;
using H00N.DataTables;
using H00N.Resources.Pools;
using ProjectF.DataTables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static ProjectF.StringUtility;

namespace ProjectF.UI.Adventures
{
    public class AdventureAreaUpgradePopupUI : UpgradePopupUI
    {
        [Space(10f)]
        [SerializeField] TMP_Text areaNameText = null;
        [SerializeField] Image areaImage = null;

        [Space(10f)]
        [SerializeField] UpgradeInfoUI timeInfoUI = null;

        private int areaID = -1;
        private Action<int, AdventureAreaUpgradePopupUI> upgradeCallback = null;

        public async void Initialize(int areaID, Action<int, AdventureAreaUpgradePopupUI> upgradeCallback)
        {
            base.Initialize();
            await InitializeUpgradeUI();

            this.areaID = areaID;
            this.upgradeCallback = upgradeCallback;
            RefreshUI();
        }

        public void RefreshUI()
        {
            int currentLevel = GameInstance.MainUser.adventureData.adventureAreas[areaID];
            AdventureLevelTableRow currentTableRow = DataTableManager.GetTable<AdventureLevelTable>().GetRow(areaID, currentLevel);
            AdventureLevelTableRow nextTableRow = DataTableManager.GetTable<AdventureLevelTable>().GetRow(areaID, currentLevel + 1);
            if (currentTableRow == null || nextTableRow == null)
                return;

            areaNameText.text = ResourceUtility.GetAdventureAreaNameLocalKey(areaID);
            new SetSprite(areaImage, ResourceUtility.GetAdventureAreaImageKey(areaID));

            // 로컬라이징 적용 해야한다.
            timeInfoUI.Initialize("탐험 시간", GetTimeString(ETimeStringType.Flexiblehms, 0, 0, 0, currentTableRow.adventureTime), GetTimeString(ETimeStringType.Flexiblehms, 0, 0, 0, nextTableRow.adventureTime));

            RefreshUpgradeUI(currentTableRow, DataTableManager.GetTable<AdventureUpgradeCostTable>().GetRowList(areaID, currentLevel));
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

            upgradeCallback?.Invoke(areaID, this);
        }
    }
}
