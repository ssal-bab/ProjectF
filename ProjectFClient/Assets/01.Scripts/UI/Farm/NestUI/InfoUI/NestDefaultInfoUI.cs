using H00N.DataTables;
using ProjectF.Datas;
using ProjectF.DataTables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectF.UI.Farms
{
    using ENestInfoUIType = NestInfoPanel.ENestInfoUIType;

    public class NestDefaultInfoUI : NestInfoUI
    {
        [SerializeField] Image nestIconImage = null;
        [SerializeField] TMP_Text nameText = null;

        private NestInfoPanel panel = null;

        public override void Initialize(UserNestData userNestData, NestUICallbackContainer callbackContainer, NestInfoPanel panel)
        {
            base.Initialize();
            this.panel = panel;

            NestTableRow tableRow = DataTableManager.GetTable<NestTable>().GetRowByLevel(userNestData.level);
            if(tableRow == null)
                return;

            RefreshUI(tableRow);
        }

        private void RefreshUI(NestTableRow tableRow)
        {
            nestIconImage.sprite = ResourceUtility.GetNestIcon(tableRow.id);
            nameText.text = $"Lv. {tableRow.level} Nest{tableRow.level}"; // 나중에 localizing 적용해야 함
        }

        public void OnTouchUpgradeButton()
        {
            panel.SetInfoUI(ENestInfoUIType.UpgradeCost);
        }
    }
}