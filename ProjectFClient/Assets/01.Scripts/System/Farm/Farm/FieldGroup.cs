using System.Collections.Generic;
using UnityEngine;
using ProjectF.Datas;

namespace ProjectF.Farms
{
    public class FieldGroup : MonoBehaviour
    {
        [SerializeField] List<Field> fields = null;
        public List<Field> Fields => fields;
        
        private int fieldGroupID = 0;
        public int FieldGroupID => fieldGroupID;

        public void Initialize(FieldGroupData fieldGroupData)
        {
            fieldGroupID = fieldGroupData.fieldGroupID;

            for(int i = 0; i < fields.Count; ++ i)
            {
                Field field = fields[i];
                if(fieldGroupData.fieldDatas.TryGetValue(i, out FieldData fieldData) == false)
                    continue;

                field.Initialize(fieldGroupData.fieldGroupID, fieldData);
            }
        }
    }
}
