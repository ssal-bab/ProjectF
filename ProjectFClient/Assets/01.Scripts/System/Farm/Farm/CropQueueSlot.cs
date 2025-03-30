using System;

namespace ProjectF.Farms
{
    public class CropQueueSlot
    {
        public int cropID = -1;
        public int count = 0;

        public Action<CropQueueSlot> OnCountChangedEvent = null;
    }
}