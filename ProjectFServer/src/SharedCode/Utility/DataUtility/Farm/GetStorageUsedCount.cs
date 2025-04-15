using System.Collections.Generic;

namespace ProjectF.Datas
{
    public struct GetStorageUsedCount
    {
        public int cropStorageUsedCount;
        // public int materialStorageUsedCount;
        public int storageUsedCount;


        public GetStorageUsedCount(UserStorageData userStorageData)
        {
            cropStorageUsedCount = 0;
            foreach(Dictionary<ECropGrade, int> storageSlot in userStorageData.cropStorage.Values)
            {
                foreach(int count in storageSlot.Values)
                    cropStorageUsedCount += count;
            }

            // materialStorageUsedCount = 0;
            // foreach(int count in userStorageData.materialStorage.Values)
            //     materialStorageUsedCount += count;

            // storageUsedCount = cropStorageUsedCount + materialStorageUsedCount;            
            storageUsedCount = cropStorageUsedCount;
        }
    }
}