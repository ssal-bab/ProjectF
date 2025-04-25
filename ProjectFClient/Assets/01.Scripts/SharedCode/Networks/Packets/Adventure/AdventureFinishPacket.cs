using System;
using ProjectF.Datas;

namespace ProjectF.Networks.Packets
{
    public class AdventureFinishRequest : RequestPacket
    {
        public override string Route => NetworkDefine.ADVENTURE_ROUTE;

        public const string POST = "AdventureFinish";
        public override string Post => POST;

        public int areaID;

        public AdventureFinishRequest(int areaID) 
        { 
            this.areaID = areaID;
        }
    }

    public class AdventureFinishResponse : ResponsePacket
    {
        public RewardData xpReward;
        public RewardData goldReward;
        public string adventureRewardUUID;
        public AdventureRewardData rewardData;
        public DateTime rewardReceiveTime;
    }
}