using ProjectF.Datas;
using ProjectF.Farms;
using ProjectF.Networks;
using ProjectF.Networks.Packets;
using UnityEngine;

namespace ProjectF.UI.Farms
{
    public class NestSlotElementUI : MonoBehaviourUI
    {
        [SerializeField] NestEggInfoUI eggInfoUI = null;
        private string eggUUID = null;

        public void Initialize(string eggUUID)
        {
            base.Initialize();
            this.eggUUID = eggUUID;
            if(string.IsNullOrEmpty(eggUUID))
                eggInfoUI.Initialize(null);
            else
                eggInfoUI.Initialize(GameInstance.MainUser.nestData.hatchingEggDatas[eggUUID]);
        }

        public void OnTouchButton()
        {
            if(string.IsNullOrEmpty(eggUUID))
                return;

            UserNestData nestData = GameInstance.MainUser.nestData;
            if(nestData.hatchingEggDatas.TryGetValue(eggUUID, out EggHatchingData hatchingData) == false)
                return;

            if(hatchingData.hatchingFinishTime > GameInstance.ServerTime)
                return;

            HatchEgg();
        }

        private async void HatchEgg()
        {
            HatchEggResponse response = await NetworkManager.Instance.SendWebRequestAsync<HatchEggResponse>(new HatchEggRequest(eggUUID));
            if(response.result != ENetworkResult.Success)
                return;

            Debug.Log($"Farmer wad born. ID : {response.farmerRewardData.rewardUUID}");
            new ApplyReward(GameInstance.MainUser, response.rewardApplyTime, response.farmerRewardData);
            GameInstance.MainUser.nestData.hatchingEggDatas.Remove(eggUUID);

            Farm farm = FarmManager.Instance.MainFarm;
            farm.FarmerQuarters.AddFarmers(response.farmerRewardData.rewardUUID);

            Initialize(null);
        }
    }
}