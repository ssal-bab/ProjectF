using System;
using ProjectF.Farms;

namespace ProjectF.UI.Farms
{
    public class CropQueueElementUI : PoolableBehaviourUI
    {
        private CropQueueSlot cropQueueSlot = null;
        private Action<CropQueueSlot> onTouchCallback = null;

        public void Initialize(CropQueueSlot cropQueueSlot, Action<CropQueueSlot> onTouchCallback)
        {
            base.Initialize();
            this.cropQueueSlot = cropQueueSlot;
            this.onTouchCallback = onTouchCallback;
        }

        public void OnTouchThis()
        {
            onTouchCallback?.Invoke(cropQueueSlot);
        }
    }
}