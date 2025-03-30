namespace ProjectF.Datas
{
    public class RewardData
    {
        public ERewardItemType rewardItemType;
        public int rewardItemID;
        public int rewardItemAmount;

        public RewardData(ERewardItemType rewardItemType, int rewardItemID, int rewardItemAmount)
        {
            this.rewardItemType = rewardItemType;
            this.rewardItemID = rewardItemID;
            this.rewardItemAmount = rewardItemAmount;
        }
    }
}