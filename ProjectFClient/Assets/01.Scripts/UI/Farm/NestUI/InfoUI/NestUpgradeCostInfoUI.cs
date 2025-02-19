using System;
using H00N.DataTables;
using ProjectF.Datas;
using ProjectF.DataTables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectF.UI.Farms
{
    using ENestInfoUIType = NestInfoPanelUI.ENestInfoUIType;

    public class NestUpgradeCostInfoUI : NestInfoUI
    {
        [SerializeField] Image nestIconImage = null;
        [SerializeField] TMP_Text nameText = null;
        [SerializeField] TMP_Text upgradeGoldText = null;
        [SerializeField] TMP_Text skipGemText = null;

        private int targetID = 0;
        private NestInfoPanelUI panel = null;
        private NestUICallbackContainer callbackContainer = null;

        public override void Initialize(UserNestData userNestData, NestUICallbackContainer callbackContainer, NestInfoPanelUI panel)
        {
            base.Initialize();
            this.panel = panel;
            this.callbackContainer = callbackContainer;

            NestTableRow tableRow = DataTableManager.GetTable<NestTable>().GetRowByLevel(userNestData.level + 1); // max level 처리해야 함
            if(tableRow == null)
            {
                panel.SetInfoUI(ENestInfoUIType.Default);
                return;
            }

            targetID = tableRow.id;
            RefreshUI(tableRow);
        }

        private void RefreshUI(NestTableRow tableRow)
        {
            nestIconImage.sprite = ResourceUtility.GetNestIcon(tableRow.id);
            nameText.text = $"Lv. {tableRow.level} Nest{tableRow.level}"; // 나중에 localizing 적용해야 함
            upgradeGoldText.text = $"{tableRow.upgradeGold}";
            skipGemText.text = $"{tableRow.skipGem}";
        }

        public void OnTouchUpgradeButton()
        {
            if(callbackContainer.UpgradeGoldCheckCallback.Invoke(targetID) == false)
                return;

            panel.SetInfoUI(ENestInfoUIType.UpgradeMaterial);
        }

        public void OnTouchSkipButton()
        {
            if (callbackContainer.SkipGemCheckCallback.Invoke(targetID) == false)
                return;

            // 정말 구매할 것인지 팝업을 띄워야 한다.
            // 우선은 바로 업그레이드로 넘기자.
            callbackContainer.UpgradeCallback.Invoke(targetID);
        }

        public void OnTouchCancelButton()
        {
            panel.SetInfoUI(ENestInfoUIType.Default);
        }
    }
}