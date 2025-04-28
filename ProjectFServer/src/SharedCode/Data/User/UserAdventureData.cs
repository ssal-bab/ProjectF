using System;
using System.Collections.Generic;

namespace ProjectF.Datas
{
    public class UserAdventureData
    {
        public Dictionary<int, int> adventureAreas = null; // <areaID, level>
        public Dictionary<int, DateTime> adventureFinishDatas = null; // <areaID, finishTime>
        public Dictionary<string, AdventureFarmerData> adventureFarmerDatas = null; // <farmerUUID, adventureFarmerData>
        public Dictionary<string, AdventureRewardData> adventureRewardDatas = null; // <adventureRewardUUID, adventureRewardData>
    }
}