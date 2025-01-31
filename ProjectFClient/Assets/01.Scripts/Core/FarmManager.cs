using System;
using System.Collections.Generic;
using ProjectF.Datas;
using ProjectF.Farms;
using ProjectF.Networks;
using ProjectF.Networks.Packets;
using UnityEngine;

namespace ProjectF
{
    public class FarmManager : MonoBehaviour
    {
        private static FarmManager instance = null;
        public static FarmManager Instance => instance;

        private const int FARM_SAVE_TICK_COUNT = 10;
        private int mainFarmSaveCounter = 0;
        private Farm mainFarm = null;

        public void Initialize()
        {
            instance = this;
            mainFarm = null;
        }

        public void Release()
        {
            instance = null;
            mainFarm = null;

            if(DateManager.Instance != null)
                DateManager.Instance.OnLateTickCycleEvent -= HandleLateTickCycleEvent;
        }

        public void RegisterFarm(Farm farm)
        {
            mainFarm = farm;
            DateManager.Instance.OnLateTickCycleEvent += HandleLateTickCycleEvent;
        }

        private void HandleLateTickCycleEvent()
        {
            if (mainFarm == null)
                return;

            mainFarmSaveCounter--;
            if(mainFarmSaveCounter > 0)
                return;

            mainFarmSaveCounter = FARM_SAVE_TICK_COUNT;
            SaveFarmAsync();
        }

        public async void SaveFarmAsync()
        {
            Dictionary<int, Dictionary<int, FieldData>> dirtiedFields = mainFarm.FlushDirtiedFields();
            if(dirtiedFields.Count <= 0)
                return;

            UpdateFarmRequest request = new UpdateFarmRequest(dirtiedFields);
            _ = await NetworkManager.Instance.SendWebRequestAsync<UpdateFarmResponse>(request);
            // UpdateFarmResponse response = await NetworkManager.Instance.SendWebRequestAsync<UpdateFarmResponse>(request);
        }
    }
}
