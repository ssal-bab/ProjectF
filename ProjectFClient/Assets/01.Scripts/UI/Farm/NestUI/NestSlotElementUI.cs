using ProjectF.Farms;
using ProjectF.Networks;
using ProjectF.Networks.Packets;
using UnityEngine;

namespace ProjectF.UI.Farms
{
    public class NestSlotElementUI : MonoBehaviourUI
    {
        [SerializeField] NestEggInfoUI eggInfoUI = null;
        private int index = 0;

        public void Initialize(int index = -1)
        {
            base.Initialize();
            this.index = index;
            if(index == -1)
                eggInfoUI.Initialize(null);
            else
                eggInfoUI.Initialize(GameInstance.MainUser.nestData.hatchingEggList[index]);
        }

        public void OnTouchButton()
        {
            if(index == -1)
                return;

            if(GameInstance.MainUser.nestData.hatchingEggList.Count <= index)
                return;

            HatchEgg();
        }

        private async void HatchEgg()
        {
            HatchEggResponse response = await NetworkManager.Instance.SendWebRequestAsync<HatchEggResponse>(new HatchEggRequest(index));
            if(response.result != ENetworkResult.Success)
                return;

            Debug.Log($"Farmer wad born. ID : {response.farmerData.farmerUUID}");
            GameInstance.MainUser.nestData.hatchingEggList.RemoveAt(response.hatchedEggIndex);
            GameInstance.MainUser.farmerData.farmerList.Add(response.farmerData.farmerUUID, response.farmerData);

            Farm farm = FarmManager.Instance.MainFarm;
            farm.FarmerQuarters.AddFarmers(response.farmerData.farmerUUID);

            Initialize(-1);
        }
    }
}