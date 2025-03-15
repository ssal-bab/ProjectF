using Cysharp.Threading.Tasks;
using H00N.Resources.Pools;
using ProjectF.Datas;
using ProjectF.Networks;
using ProjectF.Networks.Packets;
using TMPro;
using UnityEngine;

namespace ProjectF.UI.Farms
{
    public class NestDetailInfoPopupUI : MonoBehaviourUI
    {
        [SerializeField] NestEggInfoUI eggInfoUI = null;
        [SerializeField] TMP_Text indexText = null;

        private UserNestData userNestData = null;
        private int currentIndex = 0;

        public void Initialize(UserNestData nestData, int index)
        {
            base.Initialize();

            userNestData = nestData;
            currentIndex = index;
            
            RefreshUI();
        }

        private void RefreshUI()
        {
            int listCount = userNestData.hatchingEggList.Count;
            if(listCount <= 0)
            {
                OnTouchCloseButton();
                return;
            }

            currentIndex %= listCount;
            indexText.text = $"{currentIndex + 1} / {listCount}";

            EggHatchingData hatchingData = userNestData.hatchingEggList[currentIndex];
            eggInfoUI.Initialize(hatchingData);
        }

        public void OnTouchNextButton(int direction)
        {
            if(direction != -1 && direction != 1)
                return;

            int listCount = userNestData.hatchingEggList.Count;
            currentIndex = (listCount + currentIndex + direction) % listCount;

            RefreshUI();
        }

        public void OnTouchHatchButton()
        {
            HatchEgg();
        }

        public void OnTouchCloseButton()
        {
            PoolManager.DespawnAsync(gameObject.GetComponent<PoolReference>()).Forget();
        }

        private async void HatchEgg()
        {
            HatchEggResponse response = await NetworkManager.Instance.SendWebRequestAsync<HatchEggResponse>(new HatchEggRequest(currentIndex));
            if(response.result != ENetworkResult.Success)
                return;

            Debug.Log($"Farmer wad born. ID : {response.farmerData.farmerUUID}");
            GameInstance.MainUser.farmerData.farmerList.Add(response.farmerData.farmerUUID, response.farmerData);
            RefreshUI();
        }
    }
}