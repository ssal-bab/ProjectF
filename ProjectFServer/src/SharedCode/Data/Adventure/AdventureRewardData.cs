using System.Collections.Generic;

namespace ProjectF.Datas
{
    public class AdventureRewardData
    {
        public string rewardUUID;
        public int areaID;
        public List<string> farmerList;
        public Dictionary<int, List<RewardData>> rewardList;
    }
}