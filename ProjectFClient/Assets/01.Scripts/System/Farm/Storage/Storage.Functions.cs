using System.Collections.Generic;
using ProjectF.Datas;

namespace ProjectF.Farms
{
    public partial class Storage
    {
        public bool ConsumeCrop(int id, ECropGrade grade, int amount)
        {
            // 우선은 StorageData도 백업 용도로만 사용한다.
            // 그러니 바로바로 UserData에 접근해서 사용하자.
            UserStorageData storageData = GameDefine.MainUser.storageData;
            if (storageData.cropStorage.TryGetValue(id, out Dictionary<ECropGrade, int> slot) == false)
                return false;

            if (slot.TryGetValue(grade, out int count) == false)
                return false;

            if(count < amount)
                return false;

            slot[grade] -= amount;
            return true;
        }

        public void StoreCrop(int id, ECropGrade grade, int amount)
        {
            // 우선은 StorageData도 백업 용도로만 사용한다.
            // 그러니 바로바로 UserData에 접근해서 사용하자.
            UserStorageData storageData = GameDefine.MainUser.storageData;
            if (storageData.cropStorage.TryGetValue(id, out Dictionary<ECropGrade, int> slot) == false)
                return;

            if (slot.ContainsKey(grade) == false)
                return;

            slot[grade] += amount;
        }
    }
}