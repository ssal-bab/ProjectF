using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ProjectF.Datas
{
    public class FieldGroupData
    {
        public int level = 0;
        public int fieldGroupID = 0;
        public Dictionary<int, FieldData> fieldDatas = null;

        [JsonIgnore]
        public Action<int> OnLevelChangedEvent = null;
    }
}