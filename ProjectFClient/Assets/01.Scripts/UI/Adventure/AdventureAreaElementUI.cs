using System;
using UnityEngine;

namespace ProjectF.UI.Adventures
{
    public class AdventureAreaElementUI : MonoBehaviourUI
    {
        [SerializeField] int areaID = -1;
        [SerializeField] GameObject menuObject = null;

        private Action<int> upgradePopupCallback = null;
        private Action<int> adventureAreaPopupCallback = null;
        private Action<int> adventureFinishCallback = null;

        public void Initialize(Action<int> upgradePopupCallback, Action<int> adventureAreaPopupCallback, Action<int> adventureFinishCallback)
        {
            base.Initialize();
            this.upgradePopupCallback = upgradePopupCallback;
            this.adventureAreaPopupCallback = adventureAreaPopupCallback;
            this.adventureFinishCallback = adventureFinishCallback;
            menuObject.SetActive(false);
        }

        public void OnTouchThis()
        {
            // 탐험 완료 확인
            bool isAdventureFinished = true;
            if(isAdventureFinished)
                adventureFinishCallback?.Invoke(areaID);
            else
                menuObject.SetActive(true);
        }

        public void OnTouchCloseMenuButton()
        {
            menuObject.SetActive(false);
        }

        public void OnTouchUpgradeButton()
        {
            upgradePopupCallback?.Invoke(areaID);
        }

        public void OnTouchAdventureButton()
        {
            adventureAreaPopupCallback?.Invoke(areaID);
        }
    }
}
