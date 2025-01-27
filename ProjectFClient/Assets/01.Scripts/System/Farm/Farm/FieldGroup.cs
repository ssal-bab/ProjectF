using System.Collections.Generic;
using UnityEngine;
using ProjectF.Datas;

namespace ProjectF.Farms
{
    public class FieldGroup : MonoBehaviour
    {
        [SerializeField] List<Field> fields = null;
        public List<Field> Fields => fields;

        private FieldGroupData fieldGroupData = null;
        public FieldGroupData FieldGroupData => fieldGroupData;

        public void Initialize(FieldGroupData data)
        {
            fieldGroupData = data;

            for(int i = 0; i < fields.Count; ++ i)
            {
                Field field = fields[i];
                if(fieldGroupData.fieldDatas.TryGetValue(i, out FieldData fieldData) == false)
                    continue;

                field.Initialize(fieldData);
            }
        }
    }
}
