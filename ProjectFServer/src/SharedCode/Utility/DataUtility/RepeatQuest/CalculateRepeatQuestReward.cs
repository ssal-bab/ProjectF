using System;
using System.Collections.Generic;
using ProjectF.DataTables;

namespace ProjectF.Datas
{
    public struct CalculateRepeatQuestReward
    {
        public List<RewardData> rewardDataList;

        public CalculateRepeatQuestReward(RepeatQuestTableRow tableRow, ERepeatQuestType repeatQuestType, UserData userData)
        {
            rewardDataList = null;
            if(userData.repeatQuestData.repeatQuestDatas.TryGetValue(repeatQuestType, out RepeatQuestData repeatQuestData) == false)
                return;
        
            rewardDataList = new List<RewardData>(tableRow.rewardDataList);
            if(repeatQuestType == ERepeatQuestType.Crop)
            {
                CropRepeatQuestTableRow cropRepeatQuestTableRow = tableRow as CropRepeatQuestTableRow;
                int targetValue = new CalculateRepeatQuestTargetValue(tableRow, repeatQuestData.repeatCount).targetValue;
                int cropPrice = new CalculateCropPrice(repeatQuestData.actionTargetID, targetValue, userData.storageData.level).cropPrice;
                int rewardGold = (int)Math.Ceiling(cropPrice * cropRepeatQuestTableRow.rewardMultiplierByDefaultPrice);

                foreach (RewardData rewardData in rewardDataList)
                {
                    if(rewardData.rewardItemType != ERewardItemType.Gold)
                        continue;

                    rewardData.rewardItemAmount = rewardGold;
                }
            }
            
            float multiplier = 1 + tableRow.targetValueMultiplierByRepeatCount * repeatQuestData.repeatCount;
            foreach(RewardData rewardData in rewardDataList)
                rewardData.rewardItemAmount = (int)Math.Ceiling(rewardData.rewardItemAmount * multiplier);
        }
    }
}