using H00N.DataTables;
using ProjectF.Datas;
using ProjectF.DataTables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectF.UI.Farms
{
    using EStorageInfoUIType = StorageInfoPanelUI.EStorageInfoUIType;

    public class StorageUpgradeMaterialInfoUI : StorageInfoUI
    {
        [SerializeField] Image storageIconImage = null;
        [SerializeField] TMP_Text nameText = null;
        [SerializeField] TMP_Text limitCountText = null;
        // [SerializeField] TMP_Text materialCountText = null;

        private int targetID = 0;
        private StorageInfoPanelUI panel = null;
        private StorageUICallbackContainer callbackContainer = null;

        public override void Initialize(UserStorageData userStorageData, StorageUICallbackContainer callbackContainer, StorageInfoPanelUI panel)
        {
            base.Initialize();
            this.panel = panel;
            this.callbackContainer = callbackContainer;

            StorageTableRow storagetableRow = DataTableManager.GetTable<StorageTable>().GetRowByLevel(userStorageData.level + 1); // max level 처리해야 함
            if(storagetableRow == null)
            {
                panel.SetInfoUI(EStorageInfoUIType.UpgradeCost); // 다시 돌려주자
                return;
            }

            ItemTableRow costItemTableRow = DataTableManager.GetTable<ItemTable>().GetRow(storagetableRow.costItemID);
            if(costItemTableRow == null)
            {
                panel.SetInfoUI(EStorageInfoUIType.UpgradeCost); // 다시 돌려주자
                return;
            }

            targetID = storagetableRow.id;
            RefreshUI(storagetableRow, costItemTableRow);
        }

        private void RefreshUI(StorageTableRow storagetableRow, ItemTableRow costItemTableRow)
        {
            storageIconImage.sprite = ResourceUtility.GetStorageIcon(storagetableRow.id);
            nameText.text = $"Lv. {storagetableRow.level} Storage{storagetableRow.level}"; // 나중에 localizing 적용해야 함
            limitCountText.text = $"Max : {storagetableRow.storeLimit}";
            // materialCountText.text = $"{storagetableRow.costItemCount} {costItemTableRow.nameLocalKey}";
        }

        public void OnTouchUpgradeButton()
        {
            if(callbackContainer.UpgradeMaterialCheckCallback.Invoke(targetID) == false)
                return;

            callbackContainer.UpgradeCallback.Invoke(targetID);
        }

        public void OnTouchCancelButton()
        {
            panel.SetInfoUI(EStorageInfoUIType.UpgradeCost);
        }
    }
}