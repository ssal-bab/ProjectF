using System;
using ProjectF.Datas;

namespace ProjectF.Networks.Packets
{
    public class HatchEggRequest : RequestPacket
    {
        public override string Route => NetworkDefine.NEST_ROUTE;

        public const string POST = "HatchEgg";
        public override string Post => POST;

        public string eggUUID;

        public HatchEggRequest(string eggUUID) 
        { 
            this.eggUUID = eggUUID;
        }
    }

    public class HatchEggResponse : ResponsePacket
    {
        public RewardData farmerRewardData;
        public DateTime rewardApplyTime;
    }
}