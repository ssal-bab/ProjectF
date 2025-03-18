using System.Collections.Generic;
using UnityEngine;
using ProjectF.Datas;
using H00N.DataTables;
using ProjectF.DataTables;

namespace ProjectF.Farms
{
    public class FieldGroup : MonoBehaviour
    {
        [SerializeField] SpriteRenderer spriteRenderer = null;

        [Space(10f)]
        [SerializeField] List<Field> fields = null;
        public List<Field> Fields => fields;
        
        private int fieldGroupID = 0;
        public int FieldGroupID => fieldGroupID;

        public void Initialize(FieldGroupData fieldGroupData)
        {
            fieldGroupID = fieldGroupData.fieldGroupID;
            
            fieldGroupData.OnLevelChangedEvent += UpdateVisual;
            UpdateVisual(fieldGroupData.level);

            for(int i = 0; i < fields.Count; ++ i)
            {
                Field field = fields[i];
                if(fieldGroupData.fieldDatas.TryGetValue(i, out FieldData fieldData) == false)
                    continue;

                field.Initialize(fieldGroupData.fieldGroupID, fieldData);
            }
        }

        private void UpdateVisual(int level)
        {
            FieldGroupTableRow tableRow = DataTableManager.GetTable<FieldGroupTable>().GetRowByLevel(level);
            spriteRenderer.sprite = ResourceUtility.GetFieldGroupIcon(tableRow.id);
        }
    }
}
