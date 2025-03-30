using System;
using System.Collections.Generic;
using ProjectF.Networks;
using ProjectF.Networks.Packets;

namespace ProjectFServer.Networks.Packets
{
    public class AdventureResultRequest : RequestPacket
    {
        public override string Route => NetworkDefine.ADVENTURE_ROUTE;

        public const string POST = "AdventureResult";
        public override string Post => POST;

        public int areaID = 0;
        public AdventureResultRequest(int areaID)
        {
            this.areaID = areaID;
        }
    }

    [Serializable]
    public struct AdventureLootMaterialGroup
    {
        public int materialItemID;
        public int itemCount;

        public AdventureLootMaterialGroup(int itemID, int count)
        {
            materialItemID = itemID;
            itemCount = count;
        }
    }
    [Serializable]
    public struct AdventureLootSeedGroup
    {
        public int seedItemID;
        public int itemCount;

        public AdventureLootSeedGroup(int itemID, int count)
        {
            seedItemID = itemID;
            itemCount = count;
        }
    }

    public class AdventureResultResponse : ResponsePacket
    {
        public List<AdventureLootMaterialGroup> materialLootInfo;
        public List<AdventureLootSeedGroup> seedLootInfo;
    }
}