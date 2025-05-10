using System;
using H00N.DataTables;
using H00N.Resources.Pools;
using ShibaInspector.Collections;
using ProjectF.Datas;
using ProjectF.DataTables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectF.UI.Farmers
{
    public class FarmerInfoPopupUI : PoolableBehaviourUI
    {
        [SerializeField] Image iconImage = null;
        [SerializeField] TMP_Text nameText = null;

        [Space(10f)]
        [SerializeField] SerializableDictionary<EFarmerStatType, FarmerStatElementUI> statElementUIList = null;
        [SerializeField] UpgradeButtonUI upgradeButtonUI = null;

        private string farmerUUID = "";
        private Action<string, FarmerInfoPopupUI> sellCallback = null;
        private Action<string, FarmerInfoPopupUI> upgradeCallback = null;

        public void Initialize(string farmerUUID, Action<string, FarmerInfoPopupUI> sellCallback, Action<string, FarmerInfoPopupUI> upgradeCallback)
        {
            base.Initialize();
            this.farmerUUID = farmerUUID;
            this.sellCallback = sellCallback;
            this.upgradeCallback = upgradeCallback;

            RefreshUI();
        }

        private void RefreshUI()
        {
            if(GameInstance.MainUser.farmerData.farmerDatas.TryGetValue(farmerUUID, out var farmerData) == false)
                return;

            var tableRow = DataTableManager.GetTable<FarmerTable>().GetRow(farmerData.farmerID);
            if(tableRow == null)
                return;

            var levelTableRow = DataTableManager.GetTable<FarmerLevelTable>().GetRow(farmerData.farmerID, farmerData.level);
            if(levelTableRow == null)
                return;

            new SetSprite(iconImage, ResourceUtility.GetFarmerIconKey(tableRow.id));
            nameText.text = $"Lv.{farmerData.level} {farmerData.nickname ?? ResourceUtility.GetFarmerNameLocalKey(tableRow.id)}";
            
            foreach(EFarmerStatType statType in statElementUIList.Keys)
                statElementUIList[statType].Initialize();

            upgradeButtonUI.Initialize(ResourceUtility.GetFarmerMonetaIconKey(tableRow.id), levelTableRow.upgradeMonetaCost, () => {
                GameInstance.MainUser.farmerData.farmerMonetaStroage.TryGetValue(tableRow.id, out int moneta);
                return moneta >= levelTableRow.upgradeMonetaCost;
            });
        }

        public void OnTouchUpgradeButton()
        {
            if(upgradeButtonUI.UpgradePossible == false)
                return;

            // 업그레이드 패널 표시
            // 거기서도 업그레이드 버튼을 누르면 
            // upgradeCallback?.Invoke(farmerUUID, this); 를 호출한다.
        }

        public void OnTouchSellButton()
        {
            sellCallback?.Invoke(farmerUUID, this);
        }

        public void OnTouchCloseButton()
        {
            Release();
            upgradeButtonUI.Release();
            PoolManager.Despawn(this);
        }
    }
}
