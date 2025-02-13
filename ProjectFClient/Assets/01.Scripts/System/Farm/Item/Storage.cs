using System.Collections.Generic;
using ProjectF.Datas;
using UnityEngine;

namespace ProjectF.Farms
{
    public class Storage : FarmerTargetableBehaviour
    {
        [SerializeField] Transform entranceTransform = null;
        public override Vector3 TargetPosition => entranceTransform.position;

        public override bool TargetEnable => Watcher != null;

        protected override void Awake()
        {
            base.Awake();
        }

        public bool ConsumeCrop(int id, ECropGrade grade, int amount)
        {
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

        // public void StoreItem(Item item)
        // {
        //     StoreItem(item.ItemData);
        //     PoolManager.Despawn(item);
        // }

        public void StoreCrop(int id, ECropGrade grade, int amount)
        {
            UserStorageData storageData = GameDefine.MainUser.storageData;
            if (storageData.cropStorage.TryGetValue(id, out Dictionary<ECropGrade, int> slot) == false)
                return;

            if (slot.ContainsKey(grade) == false)
                return;

            slot[grade] += amount;
        }

        #if UNITY_EDITOR
        protected override void DrawGizmos()
        {
            if(entranceTransform == null)
                return;

            base.DrawGizmos();
        }
        #endif
    }
}