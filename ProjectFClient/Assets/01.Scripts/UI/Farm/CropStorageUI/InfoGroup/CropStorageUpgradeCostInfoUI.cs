using System;
using H00N.DataTables;
using ProjectF.Datas;
using ProjectF.DataTables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectF.UI.Farms
{
    using ECropStorageInfoUIType = CropStorageInfoPanel.ECropStorageInfoUIType;

    public class CropStorageUpgradeCostInfoUI : CropStorageInfoUI
    {
        [SerializeField] Image storageIconImage = null;
        [SerializeField] TMP_Text nameText = null;
        [SerializeField] TMP_Text upgradeGoldText = null;
        [SerializeField] TMP_Text skipGemText = null;

        private int targetID = 0;
        private CropStorageInfoPanel panel = null;
        private CropStorageUICallbackContainer callbackContainer = null;

        public override void Initialize(UserCropStorageData userCropStorageData, CropStorageUICallbackContainer callbackContainer, CropStorageInfoPanel panel)
        {
            base.Initialize();
            this.panel = panel;
            this.callbackContainer = callbackContainer;

            CropStorageTable cropStorageTable = DataTableManager.GetTable<CropStorageTable>();
            CropStorageTableRow tableRow = cropStorageTable.GetRowByLevel(userCropStorageData.level + 1); // max level 처리해야 함
            if(tableRow == null)
            {
                panel.SetInfoUI(ECropStorageInfoUIType.Default);
                return;
            }

            targetID = tableRow.id;
            RefreshUI(tableRow);
        }

        private void RefreshUI(CropStorageTableRow tableRow)
        {
            // storageIconImage.sprite = ResourceUtility.GetStorageIcon(tableRow.id);
            nameText.text = $"Lv. {tableRow.level} Storage{tableRow.level}"; // 나중에 localizing 적용해야 함
            upgradeGoldText.text = $"{tableRow.upgradeGold}";
            skipGemText.text = $"{tableRow.skipGem}";
        }

        public void OnTouchUpgradeButton()
        {
            if(callbackContainer.UpgradeGoldCheckCallback.Invoke(targetID) == false)
                return;

            panel.SetInfoUI(ECropStorageInfoUIType.UpgradeMaterial);
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
            panel.SetInfoUI(ECropStorageInfoUIType.Default);
        }
    }
}