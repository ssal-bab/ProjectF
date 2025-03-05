using System;
using ProjectF.Datas;

namespace ProjectF.Networks.Packets
{
    public class StorageUpgradeRequest : RequestPacket
    {
        public override string Route => NetworkDefine.STANDARD_ROUTE;

        public const string POST = "StorageUpgrade";
        public override string Post => POST;

        public StorageUpgradeRequest() { }
    }

    public class StorageUpgradeResponse : ResponsePacket
    {
        public int usedGold;
        public int usedCostItemID;
        public int usedCostItemCount;
        public int currentLevel;
    }
}