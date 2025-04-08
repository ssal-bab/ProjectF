using System;

namespace ProjectF.Datas
{
    public class CropQueueSlot
    {
        public int cropID = -1;
        public int count = 0;

        public Action<CropQueueSlot> OnCountChangedEvent = null;

        public CropQueueSlot Clone()
        {
            CropQueueSlot cropQueueSlot = new CropQueueSlot();
            cropQueueSlot.cropID = cropID;
            cropQueueSlot.count = count;
            
            return cropQueueSlot;
        }
    }
}