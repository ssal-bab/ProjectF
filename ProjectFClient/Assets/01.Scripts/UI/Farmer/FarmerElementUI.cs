using System;
using H00N.Resources;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectF.UI.Farmers
{
    public class FarmerElementUI : PoolableBehaviourUI
    {
        [SerializeField] Image dimedImage = null;
        private Color selectedColor = Color.white;
        private Action<FarmerElementUI> touchCallback = null;
        
        private string farmerUUID = "";
        public string FarmerUUID => farmerUUID;

        public void Initialize(string farmerUUID, Color selectedColor, Action<FarmerElementUI> touchCallback)
        {
            base.Initialize();
            this.farmerUUID = farmerUUID;
            this.selectedColor = selectedColor;
            this.touchCallback = touchCallback;
        }

        public void OnTouchThis()
        {
            touchCallback?.Invoke(this);
        }

        public void SetSelected(bool isSelected)
        {
            if(isSelected)
                dimedImage.color = selectedColor;
            else
                dimedImage.color = new Color(1, 1, 1, 0);
        }
    }
}
