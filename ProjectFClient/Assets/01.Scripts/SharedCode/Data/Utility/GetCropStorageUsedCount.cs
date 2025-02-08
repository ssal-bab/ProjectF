using System.Collections.Generic;

namespace ProjectF.Datas
{
    public struct GetCropStorageUsedCount
    {
        public int usedCount;

        public GetCropStorageUsedCount(UserCropStorageData userCropStorageData)
        {
            usedCount = 0;
            foreach(Dictionary<int, int> storageSlot in userCropStorageData.cropStorage.Values)
            {
                foreach(int count in storageSlot.Values)
                    usedCount += count;
            }
        }
    }
}