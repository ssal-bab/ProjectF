using System;
using Cysharp.Threading.Tasks;
using H00N.Resources.Pools;
using ProjectF.Datas;
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

        private Func<int, UniTask<int>> hatchCallback = null;

        public void Initialize(UserNestData nestData, int index, Func<int, UniTask<int>> hatchCallback)
        {
            base.Initialize();
            this.hatchCallback = hatchCallback;

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

        public async void OnTouchHatchButton()
        {
            if(hatchCallback == null)
                return;

            int bornFarmerID = await hatchCallback.Invoke(currentIndex);
            Debug.Log($"Farmer wad born. ID : {bornFarmerID}");
            
            // 여기다 팝업 띄우고
            // 팝업 종료됐을 때 RefreshUI 를 호출한다.
            // 아직은 결과 팝업이 없으니 그냥 쌩으로 호출
            RefreshUI();
        }

        public void OnTouchCloseButton()
        {
            PoolManager.DespawnAsync(gameObject.GetComponent<PoolReference>()).Forget();
        }
    }
}