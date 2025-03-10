using H00N.DataTables;
using ProjectF.Datas;
using ProjectF.DataTables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectF.UI.Farms
{
    using ENestInfoUIType = NestInfoPanelUI.ENestInfoUIType;

    public class NestUpgradeMaterialInfoUI : NestInfoUI
    {
        [SerializeField] Image nestIconImage = null;
        [SerializeField] TMP_Text nameText = null;
        // [SerializeField] TMP_Text materialCountText = null;

        private int targetID = 0;
        private NestInfoPanelUI panel = null;
        private NestUICallbackContainer callbackContainer = null;

        public override void Initialize(UserNestData userNestData, NestUICallbackContainer callbackContainer, NestInfoPanelUI panel)
        {
            base.Initialize();
            this.panel = panel;
            this.callbackContainer = callbackContainer;

            NestTableRow nestTableRow = DataTableManager.GetTable<NestTable>().GetRowByLevel(userNestData.level + 1); // max level 처리해야 함
            if(nestTableRow == null)
            {
                panel.SetInfoUI(ENestInfoUIType.UpgradeCost); // 다시 돌려주자
                return;
            }

            MaterialTableRow materialTableRow = DataTableManager.GetTable<MaterialTable>().GetRow(nestTableRow.materialID);
            if(materialTableRow == null)
            {
                panel.SetInfoUI(ENestInfoUIType.UpgradeCost); // 다시 돌려주자
                return;
            }

            targetID = nestTableRow.id;
            RefreshUI(nestTableRow, materialTableRow);
        }

        private void RefreshUI(NestTableRow nestTableRow, MaterialTableRow materialTableRow)
        {
            nestIconImage.sprite = ResourceUtility.GetNestIcon(nestTableRow.id);
            nameText.text = $"Lv. {nestTableRow.level} Nest{nestTableRow.level}"; // 나중에 localizing 적용해야 함
            // materialCountText.text = $"{nropNestTableRow.costItemCount} {costItemTableRow.nameLocalKey}";
        }

        public void OnTouchUpgradeButton()
        {
            if(callbackContainer.UpgradeMaterialCheckCallback.Invoke(targetID) == false)
                return;

            callbackContainer.UpgradeCallback.Invoke(targetID);
        }

        public void OnTouchCancelButton()
        {
            panel.SetInfoUI(ENestInfoUIType.UpgradeCost);
        }
    }
}