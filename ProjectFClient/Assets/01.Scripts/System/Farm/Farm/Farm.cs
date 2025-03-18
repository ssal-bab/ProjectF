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

        [SerializeField] Storage storage = null;
        public Storage Storage => storage;

        private CropQueue cropQueue = null;
        public CropQueue CropQueue => cropQueue;

        private void Awake()
        {
            cropQueue = new CropQueue();
        }

        private void Start()
        {
            UserData mainUser = GameInstance.MainUser;
            for (int i = 0; i < fieldGroups.Count; ++i)
            {
                FieldGroup fieldGroup = fieldGroups[i];
                if(mainUser.fieldGroupData.fieldGroupDatas.TryGetValue(i, out FieldGroupData fieldGroupData) == false)
                    continue;

                fieldGroup.Initialize(fieldGroupData);
            }

            FarmManager.Instance.RegisterFarm(this);
        }

        public void UpdateFieldGroupData()
        {
            Dictionary<int, FieldGroupData> userFieldGroupDatas = GameInstance.MainUser.fieldGroupData.fieldGroupDatas;
            foreach (FieldGroup fieldGroup in fieldGroups)
            {
                foreach (Field field in fieldGroup.Fields)
                {
                    FieldData fieldData = userFieldGroupDatas[fieldGroup.FieldGroupID].fieldDatas[field.FieldID];
                    fieldData.currentCropID = field.CurrentCropData == null ? -1 : field.CurrentCropData.TableRow.id;
                    fieldData.currentGrowth = field.Growth;
                    fieldData.fieldState = field.FieldState;
                }
            }
        }
    }
}
