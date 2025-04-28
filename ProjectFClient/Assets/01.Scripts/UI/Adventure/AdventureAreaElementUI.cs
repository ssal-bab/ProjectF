using System;
using UnityEngine;

namespace ProjectF.UI.Adventures
{
    public class AdventureAreaElementUI : MonoBehaviourUI
    {
        [SerializeField] int areaID = -1;
        public int AreaID => areaID;

        [SerializeField] GameObject menuObject = null;
        [SerializeField] GameObject lockObject = null;
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

            GameInstance.MainUser.adventureData.adventureAreas.TryGetValue(areaID, out int level);
            bool isLocked = level <= 0;
            lockObject.SetActive(isLocked);
        }

        public void OnTouchThis()
        {
            if(GameInstance.MainUser.adventureData.adventureFinishDatas.TryGetValue(areaID, out DateTime finishTime) == false)
            {
                menuObject.SetActive(true);
                return;
            }

            if(finishTime > GameInstance.ServerTime)
                menuObject.SetActive(true);
            else        
                adventureFinishCallback?.Invoke(areaID);
        }

        public void OnTouchUnlockButton()
        {
            GameInstance.MainUser.adventureData.adventureAreas.TryGetValue(areaID, out int level);
            if(level > 0)
            {
                lockObject.SetActive(false);
                return;
            }

            upgradePopupCallback?.Invoke(areaID);
        }

        public void OnTouchCloseMenuButton()
        {
            menuObject.SetActive(false);
        }

        public void OnTouchUpgradeButton()
        {
            upgradePopupCallback?.Invoke(areaID);
            menuObject.SetActive(false);
        }

        public void OnTouchAdventureButton()
        {
            adventureAreaPopupCallback?.Invoke(areaID);
            menuObject.SetActive(false);
        }
    }
}
