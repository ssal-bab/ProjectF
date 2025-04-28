using H00N.Resources;
using UnityEngine;

namespace ProjectF.UI.Farmers
{
    public class FarmerElementUI : PoolableBehaviourUI
    {
        private string farmerUUID = "";
        private Color selectedColor = Color.white;

        public void Initialize(string farmerUUID, Color selectedColor)
        {
            base.Initialize();
            this.farmerUUID = farmerUUID;
            this.selectedColor = selectedColor;
        }
    }
}
