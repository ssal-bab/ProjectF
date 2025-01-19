using System.Collections.Generic;
using UnityEngine;

namespace ProjectCoin.Farms
{
    public class Farm : MonoBehaviour
    {
        [SerializeField] EggStorage eggStorage = null;
        public EggStorage EggStorage => eggStorage;
        
        [SerializeField] CropStorage cropStorage = null;
        public CropStorage CropStorage => cropStorage;
        
        private Dictionary<CropSO, int> cropJobInfo = null;
        private Queue<CropSO> cropQueue = null;

        public bool CropQueueValid => cropQueue.Count > 0;

        private void Awake()
        {
            cropJobInfo = new Dictionary<CropSO, int>();
            cropQueue = new Queue<CropSO>();
        }

        public void EnqueueCropData(CropSO cropData)
        {
            if(cropJobInfo.ContainsKey(cropData) == false)
            {
                cropQueue.Enqueue(cropData);
                cropJobInfo.Add(cropData, 0);
            }

            cropJobInfo[cropData]++;
        }

        public CropSO DequeueCropData()
        {
            if(CropQueueValid == false)
                return null;

            CropSO cropData = cropQueue.Peek();
            cropJobInfo[cropData]--;
            if(cropJobInfo[cropData] <= 0)
            {
                cropJobInfo.Remove(cropData);
                cropQueue.Dequeue();
            }

            return cropData;
        }
    }
}
