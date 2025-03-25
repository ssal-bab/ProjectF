using System;

namespace ProjectF.UI.Farms
{
    public class SeedElementUI : PoolableBehaviourUI
    {
        private int cropID = -1;

        // <cropID>
        private Action<int> onTouchAddCallback = null;

        public void Initialize(int cropID, Action<int> onTouchAddCallback)
        {
            base.Initialize();
            this.cropID = cropID;
            this.onTouchAddCallback = onTouchAddCallback;
        }

        public void OnTouchAddButton()
        {
            onTouchAddCallback?.Invoke(cropID);
        }
    }
}