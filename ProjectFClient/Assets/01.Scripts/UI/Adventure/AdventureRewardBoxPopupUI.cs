using System.Collections.Generic;
using H00N.Extensions;
using H00N.Resources;
using H00N.Resources.Pools;
using ProjectF.Datas;
using ProjectF.Networks;
using ProjectF.Networks.Packets;
using UnityEngine;

namespace ProjectF.UI.Adventures
{
    public class AdventureRewardBoxPopupUI : PoolableBehaviourUI
    {
        [SerializeField] AddressableAsset<AdventureRewardBoxElementUI> rewardBoxElementUIPrefab = null;
        [SerializeField] Transform rewardBoxContainer = null;

        public new async void Initialize()
        {
            base.Initialize();
            await rewardBoxElementUIPrefab.InitializeAsync();

            RefreshUI();
        }

        private void RefreshUI()
        {
            rewardBoxContainer.DespawnAllChildren();
            foreach(AdventureRewardData adventureRewardData in GameInstance.MainUser.adventureData.adventureRewardDatas.Values)
            {
                foreach(int rewardIndex in adventureRewardData.rewardList.Keys)
                {
                    List<RewardData> rewardDataList = adventureRewardData.rewardList[rewardIndex];
                    AdventureRewardBoxElementUI rewardBoxElementUI = PoolManager.Spawn(rewardBoxElementUIPrefab, rewardBoxContainer);
                    rewardBoxElementUI.InitializeTransform();
                    rewardBoxElementUI.Initialize(adventureRewardData, rewardIndex, ReceiveReward);
                }
            }
        }

        private async void ReceiveReward(string adventureRewardUUID, int rewardIndex, AdventureRewardBoxElementUI ui)
        {
            AdventureRewardReceiveResponse response = await NetworkManager.Instance.SendWebRequestAsync<AdventureRewardReceiveResponse>(new AdventureRewardReceiveRequest(adventureRewardUUID, rewardIndex));
            if(response.result != ENetworkResult.Success)
                return;

            UserAdventureData adventureData = GameInstance.MainUser.adventureData;
            if(adventureData.adventureRewardDatas.TryGetValue(adventureRewardUUID, out AdventureRewardData adventureRewardData) == false)
            {
                ui.Release();
                PoolManager.Despawn(ui);
                return;
            }

            if(adventureRewardData.rewardList.TryGetValue(rewardIndex, out List<RewardData> rewardDataList) == false)
            {
                ui.Release();
                PoolManager.Despawn(ui);
                return;
            }

            new ApplyReward(GameInstance.MainUser, response.rewardApplyTime, rewardDataList);
            adventureRewardData.rewardList.Remove(rewardIndex);
            if(adventureRewardData.rewardList.Count <= 0)
                adventureData.adventureRewardDatas.Remove(adventureRewardUUID);

            ui.Release();
            PoolManager.Despawn(ui);
        }
    }
}
