using System.Collections.Generic;
using H00N.Resources;
using H00N.Resources.Pools;
using ProjectF.Datas;
using ProjectF.Networks.Packets;
using TMPro;
using UnityEngine;

namespace ProjectF.UI.Adventures
{
    public class AdventureReportPopupUI : PoolableBehaviourUI
    {
        [SerializeField] TMP_Text titleText = null;

        [Space(10f)]
        [SerializeField] List<AdventureFarmerElementUI> farmerElementUIList = null;

        [Space(10f)]
        [SerializeField] TMP_Text xpText = null;
        [SerializeField] TMP_Text goldText = null;

        [Space(10f)]
        [SerializeField] AddressableAsset<AdventureLootElementUI> lootElementUIPrefab = null;
        [SerializeField] Transform cropLootContainer = null;
        [SerializeField] Transform eggLootContainer = null;

        public void Initialize(AdventureFinishResponse adventureFinishData)
        {
            base.Initialize();

            // 일회용 UI이기 때문에 별도로 데이터를 저장하지 않고 바로 표시한다.
            titleText.text = ResourceUtility.GetAdventureAreaNameLocalKey(adventureFinishData.rewardData.areaID);

            for(int i = 0; i < farmerElementUIList.Count; ++i)
            {
                string farmerID = string.Empty;
                if(i < adventureFinishData.rewardData.farmerList.Count)
                    farmerID = adventureFinishData.rewardData.farmerList[i];

                farmerElementUIList[i].Initialize(farmerID);
            }

            xpText.text = adventureFinishData.xpReward.rewardItemAmount.ToString();
            goldText.text = adventureFinishData.goldReward.rewardItemAmount.ToString();
            foreach(List<RewardData> reward in adventureFinishData.rewardData.rewardList.Values)
            {
                foreach(RewardData rewardElement in reward)
                {
                    if(rewardElement.rewardItemType == ERewardItemType.Seed)
                    {
                        AdventureLootElementUI cropLootElement = PoolManager.Spawn(lootElementUIPrefab, cropLootContainer);
                        cropLootElement.InitializeTransform();
                        cropLootElement.Initialize(ELootItemType.Crop, rewardElement.rewardItemID);
                    }
                    else if(rewardElement.rewardItemType == ERewardItemType.Egg)
                    {
                        AdventureLootElementUI eggLootElement = PoolManager.Spawn(lootElementUIPrefab, eggLootContainer);
                        eggLootElement.InitializeTransform();
                        eggLootElement.Initialize(ELootItemType.Egg, rewardElement.rewardItemID);
                    }
                }
            }
        }

        public void OnTouchCloseButton()
        {
            base.Release();
            PoolManager.Despawn(this);
        }
    }
}
