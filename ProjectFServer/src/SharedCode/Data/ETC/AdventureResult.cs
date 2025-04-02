using System;
using System.Collections.Generic;

namespace ProjectF.Datas
{
    [Serializable]
    public struct AdventureResultPack
    {
        public List<RewardData> materialLootInfo;
        public List<RewardData> seedLootInfo;

        public AdventureResultPack(List<RewardData> materialLootInfo, List<RewardData> seedLootInfo)
        {
            this.materialLootInfo = materialLootInfo;
            this.seedLootInfo = seedLootInfo;
        }
    }
}