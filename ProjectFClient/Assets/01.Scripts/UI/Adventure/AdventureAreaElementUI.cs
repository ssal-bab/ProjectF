using System;
using UnityEngine;

namespace ProjectF.UI.Adventures
{
    public class AdventureAreaElementUI : MonoBehaviourUI
    {
        [SerializeField] int areaID = -1;
        [SerializeField] GameObject menuObject = null;

        private Action<int> upgradeMenuCallback = null;
        private Action<int> adventureMenuCallback = null;

        public void Initialize(Action<int> upgradeMenuCallback, Action<int> adventureMenuCallback)
        {
            base.Initialize();
            this.upgradeMenuCallback = upgradeMenuCallback;
            this.adventureMenuCallback = adventureMenuCallback;

            menuObject.SetActive(false);
        }

        public void OnTouchOpenMenuButton()
        {
            menuObject.SetActive(true);
        }

        public void OnTouchCloseMeneButton()
        {
            menuObject.SetActive(false);
        }

        public void OnTouchUpgradeButton()
        {
            upgradeMenuCallback?.Invoke(areaID);
        }

        public void OnTouchAdventureButton()
        {
            adventureMenuCallback?.Invoke(areaID);
        }
    }
}
