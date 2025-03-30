using System;
using System.Collections.Generic;

namespace ProjectF.Datas
{
    public class UserAdventureData
    {
        public Dictionary<int, AdventureData> adventureList = null;
        public Dictionary<int, DateTime> inAdventureAreaList = null;
        public Dictionary<int, List<string>> inExploreFarmerList = null;
        public List<string> allFarmerinExploreList = null;
    }
}