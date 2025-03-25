using System.Collections;
using System.Collections.Generic;

namespace ProjectF.Farms
{
    public class CropQueue : IEnumerable<CropQueueSlot>
    {
        private Queue<CropQueueSlot> cropQueue = null;

        public bool CropQueueValid => cropQueue.Count > 0;

        public CropQueue()
        {
            cropQueue = new Queue<CropQueueSlot>();
        }

        public void EnqueueCrop(int cropID)
        {
            CropQueueSlot slot = PeekSlot();
            if(slot == null || slot.cropID != cropID)
            {
                slot = new CropQueueSlot(){
                    cropID = cropID,
                    count = 0
                };
                cropQueue.Enqueue(slot);
            }

            slot.count++;
        }

        public int DequeueCropData()
        {
            CropQueueSlot slot = PeekSlot();
            slot.count--;
            if(slot.count <= 0)
                cropQueue.Dequeue();

            return slot.cropID;
        }

        private CropQueueSlot PeekSlot()
        {
            if(CropQueueValid == false)
                return null;

            return cropQueue.Peek();
        }

        public IEnumerator<CropQueueSlot> GetEnumerator() => cropQueue.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}