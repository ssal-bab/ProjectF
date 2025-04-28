using System;
using UnityEngine;

namespace ProjectF.UI.Adventures
{
    public class AdventureFarmerElementUI : MonoBehaviourUI
    {
        private string farmerID = string.Empty;

        public void Initialize(int areaID, int index)
        {
            base.Initialize();
        }

        public void Initialize(string farmerID)
        {
            base.Initialize();
            this.farmerID = farmerID;

            if(string.IsNullOrEmpty(farmerID))
                gameObject.SetActive(false);
        }

        public bool TryGetFarmerID(out string farmerID)
        {
            farmerID = this.farmerID;
            return string.IsNullOrEmpty(farmerID) == false;
        }
    }
}
