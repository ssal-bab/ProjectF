using System.Collections.Generic;
using H00N.DataTables;
using H00N.Resources;
using H00N.Resources.Pools;
using ProjectF.Datas;
using ProjectF.DataTables;
using ProjectF.Networks;
using ProjectF.Networks.Packets;
using UnityEngine;

namespace ProjectF.UI.Adventures
{
    public class AdventurePopupUI : PoolableBehaviourUI
    {
        [SerializeField] AddressableAsset<AdventureAreaUpgradePopupUI> upgradePopupUIPrefab = null;
        [SerializeField] AddressableAsset<AdventureAreaPopupUI> areaPopupUIPrefab = null;
        
        [Space(10f)]
        [SerializeField] List<AdventureAreaElementUI> areaElementUIList = null;

        public new async void Initialize()
        {
            base.Initialize();
            await areaPopupUIPrefab.InitializeAsync();
            await upgradePopupUIPrefab.InitializeAsync();

            foreach (AdventureAreaElementUI elementUI in areaElementUIList)
                elementUI.Initialize(OpenUpgradePopupUI, OpenAreaPopupUI);
        }

        private void OpenUpgradePopupUI(int areaID)
        {
            AdventureAreaUpgradePopupUI upgradePopupUI = PoolManager.Spawn(upgradePopupUIPrefab, GameDefine.ContentPopupFrame);
            upgradePopupUI.StretchRect();
            upgradePopupUI.Initialize(areaID, UpgradeAdventureArea);
        }

        private void OpenAreaPopupUI(int areaID)
        {
            AdventureAreaPopupUI areaPopupUI = PoolManager.Spawn(areaPopupUIPrefab, GameDefine.ContentPopupFrame);
            areaPopupUI.StretchRect();
            areaPopupUI.Initialize(areaID, StartAdventureAsync);
        }

        public void OnTouchCloseButton()
        {
            base.Release();
            PoolManager.Despawn(this);
        }

        private async void UpgradeAdventureArea(int areaID, AdventureAreaUpgradePopupUI ui)
        {
            AdventureAreaUpgradeResponse response = await NetworkManager.Instance.SendWebRequestAsync<AdventureAreaUpgradeResponse>(new AdventureAreaUpgradeRequest(areaID));
            if (response.result != ENetworkResult.Success)
                return;

            UserData mainUser = GameInstance.MainUser;

            AdventureLevelTableRow tableRow = DataTableManager.GetTable<AdventureLevelTable>().GetRow(areaID, response.currentLevel - 1);
            mainUser.monetaData.gold -= tableRow.gold;

            new ApplyUpgradeCost<NestUpgradeCostTableRow>(mainUser.storageData, DataTableManager.GetTable<NestUpgradeCostTable>().GetRowListByLevel(response.currentLevel - 1));

            mainUser.adventureData.adventureAreas[areaID] = response.currentLevel;

            if(ui != null)
                ui.OnTouchCloseButton();
        }

        private async void StartAdventureAsync(int areaID, List<string> farmerList, AdventureAreaPopupUI ui)
        {
            AdventureStartResponse response = await NetworkManager.Instance.SendWebRequestAsync<AdventureStartResponse>(new AdventureStartRequest(areaID, farmerList));
            if (response.result != ENetworkResult.Success)
                return;

            UserAdventureData adventureData = GameInstance.MainUser.adventureData;
            adventureData.adventureFinishDatas[areaID] = response.finishTime;
            foreach(var farmerID in farmerList)
            {
                adventureData.adventureFarmerDatas[farmerID] = new AdventureFarmerData() {
                    farmerUUID = farmerID,
                    areaID = areaID
                };
            }

            if(ui != null)
                ui.Initialize(areaID, StartAdventureAsync);
        }
    }
}
