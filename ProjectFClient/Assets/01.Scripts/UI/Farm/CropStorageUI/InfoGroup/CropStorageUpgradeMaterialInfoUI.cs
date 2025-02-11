using H00N.DataTables;
using ProjectF.Datas;
using ProjectF.DataTables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectF.UI.Farms
{
    using ECropStorageInfoUIType = CropStorageInfoPanel.ECropStorageInfoUIType;

    public class CropStorageUpgradeMaterialInfoUI : CropStorageInfoUI
    {
        [SerializeField] Image storageIconImage = null;
        [SerializeField] TMP_Text nameText = null;
        [SerializeField] TMP_Text limitCountText = null;
        // [SerializeField] TMP_Text materialCountText = null;

        private int targetID = 0;
        private CropStorageInfoPanel panel = null;
        private CropStorageUICallbackContainer callbackContainer = null;

        public override void Initialize(UserCropStorageData userCropStorageData, CropStorageUICallbackContainer callbackContainer, CropStorageInfoPanel panel)
        {
            base.Initialize();
            this.panel = panel;
            this.callbackContainer = callbackContainer;

            CropStorageTable cropStorageTable = DataTableManager.GetTable<CropStorageTable>();
            CropStorageTableRow cropStoragetableRow = cropStorageTable.GetRowByLevel(userCropStorageData.level + 1); // max level 처리해야 함
            if(cropStoragetableRow == null)
                return;

            ItemTable itemTable = DataTableManager.GetTable<ItemTable>();
            ItemTableRow costItemTableRow = itemTable.GetRow(cropStoragetableRow.costItemID);
            if(costItemTableRow == null)
                return;

            targetID = cropStoragetableRow.id;
            RefreshUI(cropStoragetableRow, costItemTableRow);
        }

        private void RefreshUI(CropStorageTableRow cropStoragetableRow, ItemTableRow costItemTableRow)
        {
            storageIconImage.sprite = ResourceUtility.GetStorageIcon(cropStoragetableRow.id);
            nameText.text = $"Lv. {cropStoragetableRow.level} Storage{cropStoragetableRow.level}"; // 나중에 localizing 적용해야 함
            limitCountText.text = $"Max : {cropStoragetableRow.storeLimit}";
            // materialCountText.text = $"{cropStoragetableRow.costItemCount} {costItemTableRow.nameLocalKey}";
        }

        public void OnTouchUpgradeButton()
        {
            if(callbackContainer.UpgradeMaterialCheckCallback.Invoke(targetID) == false)
                return;

            callbackContainer.UpgradeCallback.Invoke(targetID);
        }

        public void OnTouchCancelButton()
        {
            panel.SetInfoUI(ECropStorageInfoUIType.UpgradeCost);
        }
    }
}