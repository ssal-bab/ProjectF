using System;
using ProjectF.Datas;

namespace ProjectF.Networks.Packets
{
    public class NestUpgradeRequest : RequestPacket
    {
        public override string Route => NetworkDefine.STANDARD_ROUTE;

        public const string POST = "NestUpgrade";
        public override string Post => POST;

        public NestUpgradeRequest() { }
    }

    public class NestUpgradeResponse : ResponsePacket
    {
        public int usedGold;
        public int usedCostItemID;
        public int usedCostItemCount;
        public int currentLevel;
    }
}