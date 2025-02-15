using System.Collections.Generic;

namespace ProjectF.Datas
{
    public class UserStorageData
    {
        public int level;

        // <id, <grade, count>>
        public Dictionary<int, Dictionary<ECropGrade, int>> cropStorage;
        public Dictionary<int, int> materialStorage;
    }
}