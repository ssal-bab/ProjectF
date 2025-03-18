using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ProjectF.Datas
{
    public class UserNestData
    {
        public int level = 0;
        public List<EggHatchingData> hatchingEggList = null;

        [JsonIgnore]
        public Action<int> OnLevelChangedEvent = null;
    }
}