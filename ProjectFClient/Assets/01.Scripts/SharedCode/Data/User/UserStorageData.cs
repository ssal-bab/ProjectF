using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ProjectF.Datas
{
    public class UserStorageData
    {
        public int level;

        // <id, <grade, count>>
        public Dictionary<int, Dictionary<ECropGrade, int>> cropStorage;
        // public Dictionary<int, int> materialStorage;

        [JsonIgnore]
        public Action<int> OnLevelChangedEvent = null;
    }
}