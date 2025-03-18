using Cysharp.Threading.Tasks;
using ProjectF.Farms;
using ProjectF.Networks;
using ProjectF.Networks.Packets;
using UnityEngine;

namespace ProjectF
{
    public class FarmManager
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
            mainFarmSaveCounter = FARM_SAVE_TICK_COUNT;
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

        public void SaveFarmAsync()
        {
            mainFarm.UpdateFieldGroupData();

            Debug.Log("[FarmManager::SaveFarmAsync] Save Farm Data");
            UpdateFarmRequest request = new UpdateFarmRequest(GameInstance.MainUser.fieldGroupData.fieldGroupDatas);
            NetworkManager.Instance.SendWebRequestAsync<UpdateFarmResponse>(request).Forget();
            // UpdateFarmResponse response = await NetworkManager.Instance.SendWebRequestAsync<UpdateFarmResponse>(request);
        }
    }
}
