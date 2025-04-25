using System;

namespace ProjectF.Datas
{
    public class RewardData
    {
        public ERewardItemType rewardItemType;
        public int rewardItemID;
        public int rewardItemAmount;
        public string rewardUUID;

        public RewardData(ERewardItemType rewardItemType, int rewardItemID, int rewardItemAmount, string rewardUUID)
        {
            this.rewardItemType = rewardItemType;
            this.rewardItemID = rewardItemID;
            this.rewardItemAmount = rewardItemAmount;
            this.rewardUUID = rewardUUID;
        }
    }
}