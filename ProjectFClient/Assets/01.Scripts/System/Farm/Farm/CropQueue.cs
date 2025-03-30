using System.Collections;
using System.Collections.Generic;

namespace ProjectF.Farms
{
    public class CropQueue : IEnumerable<CropQueueSlot>
    {
        private List<CropQueueSlot> cropQueue = null;

        public bool CropQueueValid => cropQueue.Count > 0;

        public CropQueue()
        {
            cropQueue = new List<CropQueueSlot>();
        }

        public void EnqueueCrop(int cropID)
        {
            CropQueueSlot slot = LastSlot();
            if(slot == null || slot.cropID != cropID)
            {
                slot = new CropQueueSlot(){
                    cropID = cropID,
                    count = 0
                };
                cropQueue.Add(slot);
            }

            slot.count++;
            slot.OnCountChangedEvent?.Invoke(slot);
        }

        public void RemoveFromCropQueue(CropQueueSlot slot)
        {
            slot.count--;
            slot.OnCountChangedEvent?.Invoke(slot);

            if(slot.count <= 0)
                cropQueue.Remove(slot);
        }

        public int DequeueCropData()
        {
            CropQueueSlot slot = PeekSlot();
            slot.count--;
            if(slot.count <= 0)
                cropQueue.RemoveAt(0);

            return slot.cropID;
        }

        public CropQueueSlot PeekSlot()
        {
            if(CropQueueValid == false)
                return null;

            return cropQueue[0];
        }

        public CropQueueSlot LastSlot()
        {
            if(CropQueueValid == false)
                return null;

            return cropQueue[cropQueue.Count - 1];
        }

        public IEnumerator<CropQueueSlot> GetEnumerator() => cropQueue.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}