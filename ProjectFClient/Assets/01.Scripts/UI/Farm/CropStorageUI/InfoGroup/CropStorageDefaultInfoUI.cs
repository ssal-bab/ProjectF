using H00N.DataTables;
using ProjectF.Datas;
using ProjectF.DataTables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectF.UI.Farms
{
    using ECropStorageInfoUIType = CropStorageInfoPanel.ECropStorageInfoUIType;

    public class CropStorageDefaultInfoUI : CropStorageInfoUI
    {
        [SerializeField] Image storageIconImage = null;
        [SerializeField] TMP_Text nameText = null;
        [SerializeField] TMP_Text limitCountText = null;
        [SerializeField] TMP_Text usedCountText = null;

        private CropStorageInfoPanel panel = null;

        public override void Initialize(UserCropStorageData userCropStorageData, CropStorageUICallbackContainer callbackContainer, CropStorageInfoPanel panel)
        {
            base.Initialize();
            this.panel = panel;

            CropStorageTable cropStorageTable = DataTableManager.GetTable<CropStorageTable>();
            CropStorageTableRow tableRow = cropStorageTable.GetRowByLevel(userCropStorageData.level);;
            if(tableRow == null)
                return;

            RefreshUI(tableRow, new GetCropStorageUsedCount(userCropStorageData).usedCount);
        }

        private void RefreshUI(CropStorageTableRow tableRow, int usedCount)
        {
            storageIconImage.sprite = ResourceUtility.GetStorageIcon(tableRow.id);
            nameText.text = $"Lv. {tableRow.level} Storage{tableRow.level}"; // 나중에 localizing 적용해야 함
            limitCountText.text = $"Max : {tableRow.storeLimit}";
            usedCountText.text = $"Used : {usedCount}";
        }

        public void OnTouchUpgradeButton()
        {
            panel.SetInfoUI(ECropStorageInfoUIType.UpgradeCost);
        }
    }
}