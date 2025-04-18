using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ProjectF.Datas;
using UnityEngine;

namespace ProjectF.Farms
{
    public class Farm : MonoBehaviour, IEnumerable<Field>
    {
        [SerializeField] List<FieldGroup> fieldGroups = null;

        [SerializeField] Storage storage = null;
        public Storage Storage => storage;

        [SerializeField] Nest nest = null;
        public Nest Nest => nest;

        [SerializeField] FarmerQuarters farmerQuarters = null;
        public FarmerQuarters FarmerQuarters => farmerQuarters;

        private CropQueue cropQueue = null;
        public CropQueue CropQueue => cropQueue;

        private async void Start()
        {
            UserData mainUser = GameInstance.MainUser;
            SetCropQueue(mainUser.seedPocketData.cropQueue);

            for (int i = 0; i < fieldGroups.Count; ++i)
            {
                FieldGroup fieldGroup = fieldGroups[i];
                if(mainUser.fieldGroupData.fieldGroupDatas.TryGetValue(i, out FieldGroupData fieldGroupData) == false)
                    continue;

                fieldGroup.Initialize(fieldGroupData);
            }

            FarmManager.Instance.RegisterFarm(this);

            storage.Initialize();
            nest.Initialize();
            await farmerQuarters.InitializeAsync();
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

        public void SetCropQueue(List<CropQueueSlot> cropQueue)
        {
            this.cropQueue = new CropQueue(cropQueue);
        }

        public IEnumerator<Field> GetEnumerator() => fieldGroups.SelectMany(i => i.Fields).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
