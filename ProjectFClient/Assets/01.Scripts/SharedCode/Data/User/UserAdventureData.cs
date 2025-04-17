using System.Collections.Generic;

namespace ProjectF.Datas
{
    public class UserAdventureData
    {
        public Dictionary<int, int> adventureAreas = null; // <areaID, level>
        public Dictionary<int, AdventureProgressData> adventureProgressDatas = null;
    }
}