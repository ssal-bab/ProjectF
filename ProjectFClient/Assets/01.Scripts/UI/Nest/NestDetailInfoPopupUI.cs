using System;
using Cysharp.Threading.Tasks;
using H00N.Resources.Pools;
using ProjectF.Datas;
using ProjectF.Networks;
using ProjectF.Networks.Packets;
using TMPro;
using UnityEngine;

namespace ProjectF.UI.Nests
{
    public class NestDetailInfoPopupUI : MonoBehaviourUI
    {
        [SerializeField] NestEggInfoUI eggInfoUI = null;
        [SerializeField] TMP_Text indexText = null;

        private int currentIndex = 0;
        private string eggUUID = null;
        private Action<int> shiftCallback = null;

        public void Initialize(int currentIndex, string eggUUID, Action<int> shiftCallback)
        {
            base.Initialize();

            this.currentIndex = currentIndex;
            this.eggUUID = eggUUID;
            this.shiftCallback = shiftCallback;

            RefreshUI();
        }

        private void RefreshUI()
        {
            UserNestData userNestData = GameInstance.MainUser.nestData;
            int listCount = userNestData.hatchingEggDatas.Count;
            if(listCount <= 0)
            {
                OnTouchCloseButton();
                return;
            }

            currentIndex %= listCount;
            indexText.text = $"{currentIndex + 1} / {listCount}";

            EggHatchingData hatchingData = userNestData.hatchingEggDatas[eggUUID];
            eggInfoUI.Initialize(hatchingData);
        }

        public void OnTouchNextButton(int direction)
        {
            if(direction != -1 && direction != 1)
                return;

            shiftCallback?.Invoke(direction);

            // int listCount = GameInstance.MainUser.nestData.hatchingEggDatas.Count;
            // currentIndex = (listCount + currentIndex + direction) % listCount;


            // RefreshUI();
        }

        public void OnTouchHatchButton()
        {
            HatchEgg();
        }

        public void OnTouchCloseButton()
        {
            PoolManager.Despawn(gameObject.GetComponent<PoolReference>());
        }

        private async void HatchEgg()
        {
            HatchEggResponse response = await NetworkManager.Instance.SendWebRequestAsync<HatchEggResponse>(new HatchEggRequest(eggUUID));
            if(response.result != ENetworkResult.Success)
                return;

            Debug.Log($"Farmer wad born. ID : {response.farmerRewardData.rewardUUID}");
            new ApplyReward(GameInstance.MainUser, GameInstance.ServerTime, response.farmerRewardData);
            RefreshUI();
        }
    }
}