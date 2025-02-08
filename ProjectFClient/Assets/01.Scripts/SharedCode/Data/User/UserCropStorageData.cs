using System.Collections.Generic;

namespace ProjectF.Datas
{
    public class UserCropStorageData
    {
        public int level;

        // <id, <grade, count>>
        public Dictionary<int, Dictionary<int, int>> cropStorage;
        public Dictionary<int, int> materialStorage;
    }
}