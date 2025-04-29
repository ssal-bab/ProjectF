using System;
using H00N.Resources;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectF.UI.Farmers
{
    public class FarmerInfoPopupUI : PoolableBehaviourUI
    {
        private string farmerUUID = "";
        private Action<string, FarmerInfoPopupUI> sellCallback = null;

        public void Initialize(string farmerUUID, Action<string, FarmerInfoPopupUI> sellCallback)
        {
            base.Initialize();
            this.farmerUUID = farmerUUID;
            this.sellCallback = sellCallback;
        }

        public void OnTouchSellButton()
        {
            sellCallback?.Invoke(farmerUUID, this);
        }
    }
}
