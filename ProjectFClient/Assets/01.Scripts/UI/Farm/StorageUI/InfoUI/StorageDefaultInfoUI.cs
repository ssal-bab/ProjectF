using H00N.DataTables;
using ProjectF.Datas;
using ProjectF.DataTables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectF.UI.Farms
{
    using EStorageInfoUIType = StorageInfoPanel.EStorageInfoUIType;

    public class StorageDefaultInfoUI : StorageInfoUI
    {
        [SerializeField] Image storageIconImage = null;
        [SerializeField] TMP_Text nameText = null;
        [SerializeField] TMP_Text limitCountText = null;
        [SerializeField] TMP_Text usedCountText = null;

        private StorageInfoPanel panel = null;

        public override void Initialize(UserStorageData userStorageData, StorageUICallbackContainer callbackContainer, StorageInfoPanel panel)
        {
            base.Initialize();
            this.panel = panel;

            StorageTable storageTable = DataTableManager.GetTable<StorageTable>();
            StorageTableRow tableRow = storageTable.GetRowByLevel(userStorageData.level);;
            if(tableRow == null)
                return;

            RefreshUI(tableRow, new GetStorageUsedCount(userStorageData).storageUsedCount);
        }

        private void RefreshUI(StorageTableRow tableRow, int usedCount)
        {
            storageIconImage.sprite = ResourceUtility.GetStorageIcon(tableRow.id);
            nameText.text = $"Lv. {tableRow.level} Storage{tableRow.level}"; // 나중에 localizing 적용해야 함
            limitCountText.text = $"Max : {tableRow.storeLimit}";
            usedCountText.text = $"Used : {usedCount}";
        }

        public void OnTouchUpgradeButton()
        {
            panel.SetInfoUI(EStorageInfoUIType.UpgradeCost);
        }
    }
}