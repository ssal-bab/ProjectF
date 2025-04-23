using System.Collections.Generic;
using ProjectF.Datas;

namespace ProjectF.Networks.Packets
{
    public class AdventureReceiveRewardRequest : RequestPacket
    {
        public override string Route => NetworkDefine.ADVENTURE_ROUTE;

        public const string POST = "AdventureReceiveReward";
        public override string Post => POST;

        public int areaID;

        public AdventureReceiveRewardRequest(int areaID) 
        { 
            this.areaID = areaID;
        }
    }

    public class AdventureReceiveRewardResponse : ResponsePacket
    {
        public RewardData xpReward;
        public RewardData goldReward;
        public List<List<RewardData>> rewardList;
    }
}