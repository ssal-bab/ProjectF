using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using ProjectF.Datas;
using ProjectF.Networks;
using ProjectF.Networks.Packets;
using UnityEngine;

namespace ProjectF.Farms
{
    public class Farm : MonoBehaviour
    {
        [SerializeField] List<FieldGroup> fieldGroups = null;

        [SerializeField] EggStorage eggStorage = null;
        public EggStorage EggStorage => eggStorage;
        
        [SerializeField] CropStorage cropStorage = null;
        public CropStorage CropStorage => cropStorage;

        private CropQueue cropQueue = null;
        public CropQueue CropQueue => cropQueue;

        private void Awake()
        {
            cropQueue = new CropQueue();
        }

        private void Start()
        {
            UserData mainUser = GameDefine.MainUser;
            for (int i = 0; i < fieldGroups.Count; ++i)
            {
                FieldGroup fieldGroup = fieldGroups[i];
                if(mainUser.fieldData.fieldGroupDatas.TryGetValue(i, out FieldGroupData fieldGroupData) == false)
                    continue;

                fieldGroup.Initialize(fieldGroupData);
            }
        }

        public Dictionary<int, Dictionary<int, FieldData>> FlushDirtiedFields()
        {
            Dictionary<int, Dictionary<int, FieldData>> dirtiedFields = new Dictionary<int, Dictionary<int, FieldData>>();
            foreach(FieldGroup fieldGroup in fieldGroups)
            {
                int fieldGroupID = fieldGroup.FieldGroupData.fieldGroupID;
                foreach(Field field in fieldGroup.Fields)
                {
                    if(field.IsDirty == false)
                        continue;

                    if(dirtiedFields.TryGetValue(fieldGroupID, out Dictionary<int, FieldData> fields) == false)
                    {
                        fields = new Dictionary<int, FieldData>();
                        dirtiedFields.Add(fieldGroupID, fields);
                    }

                    FieldData fieldData = field.FieldData;
                    fields.Add(fieldData.fieldID, fieldData);
                    field.ClearDirty();
                }
            }

            return dirtiedFields;
        }
    }
}
