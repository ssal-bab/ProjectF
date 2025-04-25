using System;
using H00N.DataTables;
using H00N.Extensions;
using H00N.Resources;
using H00N.Resources.Pools;
using ProjectF.Datas;
using ProjectF.DataTables;
using TMPro;
using UnityEngine;
using static ProjectF.StringUtility;

namespace ProjectF.UI.Adventures
{
    public class AdventureRewardBoxElementUI : PoolableBehaviourUI
    {
        [SerializeField] AddressableAsset<AdventureLootElementUI> lootElementUIPrefab = null;
        [SerializeField] TMP_Text titleText = null;
        [SerializeField] TMP_Text timeText = null;
        [SerializeField] Transform lootElementUIContainer = null;

        private AdventureRewardData adventureRewardData;
        private int rewardIndex;
        private Action<string, int, AdventureRewardBoxElementUI> receiveRewardCallback;

        public async void Initialize(AdventureRewardData adventureRewardData, int rewardIndex, Action<string, int, AdventureRewardBoxElementUI> receiveRewardCallback)
        {
            base.Initialize();
            await lootElementUIPrefab.InitializeAsync();
        
            this.adventureRewardData = adventureRewardData;
            this.rewardIndex = rewardIndex;
            this.receiveRewardCallback = receiveRewardCallback;

            RefreshUI();
        }

        public new void Release()
        {
            base.Release();
        }

        private void RefreshUI()
        {
            titleText.text = ResourceUtility.GetAdventureAreaNameLocalKey(adventureRewardData.areaID);

            if(GameInstance.MainUser.adventureData.adventureAreas.TryGetValue(adventureRewardData.areaID, out int level) != false)
            {
                AdventureLevelTableRow tableRow = DataTableManager.GetTable<AdventureLevelTable>().GetRow(adventureRewardData.areaID, level - 1);
                timeText.text = GetTimeString(ETimeStringType.Flexiblehms, 0, 0, 0, tableRow.adventureTime);
            }
            else
            {
                timeText.text = GetTimeString(ETimeStringType.Flexiblehms, 0, 0, 0, 0);
            }

            lootElementUIContainer.DespawnAllChildren();
            foreach(RewardData rewardData in adventureRewardData.rewardList[rewardIndex])
            {
                AdventureLootElementUI lootElementUI = PoolManager.Spawn(lootElementUIPrefab, lootElementUIContainer);
                lootElementUI.InitializeTransform();
                lootElementUI.Initialize(rewardData.rewardItemType == ERewardItemType.Seed ? ELootItemType.Crop : ELootItemType.Egg, rewardData.rewardItemID, rewardData.rewardItemAmount);
            }
        }

        public void OnTouchReceiveButton()
        {
            receiveRewardCallback?.Invoke(adventureRewardData.rewardUUID, rewardIndex, this);
        }
    }
}
