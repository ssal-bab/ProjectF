using System.Collections;
using System.Collections.Generic;

namespace ProjectF.Datas
{
    public class CropQueue : IEnumerable<CropQueueSlot>
    {
        private List<CropQueueSlot> cropQueue = null;

        public bool CropQueueValid => cropQueue.Count > 0;
        public int Count => cropQueue.Count;

        public CropQueueSlot this[int index] => cropQueue[index];

        public CropQueue()
        {
            cropQueue = new List<CropQueueSlot>();
        }

        public CropQueue(List<CropQueueSlot> source)
        {
            cropQueue = source;
        }

        public void EnqueueCrop(int cropID, int count = 1)
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

            slot.count += count;
            slot.OnCountChangedEvent?.Invoke(slot);
        }

        public void RemoveFromCropQueue(int slotIndex, int count = 1)
        {
            if(slotIndex < 0 || slotIndex > cropQueue.Count - 1)
                return;

            CropQueueSlot slot = cropQueue[slotIndex];
            slot.count -= count;
            slot.count -= count;
            slot.OnCountChangedEvent?.Invoke(slot);

            if(slot.count <= 0)
                cropQueue.RemoveAt(slotIndex);
        }

        public int DequeueCropData(int count = 1)
        {
            CropQueueSlot slot = PeekSlot();
            slot.count -= count;
            slot.OnCountChangedEvent?.Invoke(slot);

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

        public int IndexOf(CropQueueSlot cropQueueSlot) => cropQueue.IndexOf(cropQueueSlot);
        public void Clear() => cropQueue.Clear();

        public IEnumerator<CropQueueSlot> GetEnumerator() => cropQueue.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}