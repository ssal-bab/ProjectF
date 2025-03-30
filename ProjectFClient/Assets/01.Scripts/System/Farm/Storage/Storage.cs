using System.Collections.Generic;
using H00N.DataTables;
using ProjectF.Datas;
using ProjectF.DataTables;
using UnityEngine;

namespace ProjectF.Farms
{
    public partial class Storage : FarmerTargetableBehaviour
    {
        [SerializeField] SpriteRenderer spriteRenderer = null;
        [SerializeField] Transform entranceTransform = null;
        public override Vector3 TargetPosition => entranceTransform.position;

        public void Initialize()
        {
            UserStorageData storageData = GameInstance.MainUser.storageData;
            storageData.OnLevelChangedEvent += UpdateVisual;
            UpdateVisual(storageData.level);
        }

        private void UpdateVisual(int level)
        {
            StorageTableRow tableRow = DataTableManager.GetTable<StorageTable>().GetRowByLevel(level);
            spriteRenderer.sprite = ResourceUtility.GetStorageIcon(tableRow.id);
        }

        public bool ConsumeCrop(int id, ECropGrade grade, int amount)
        {
            // 우선은 StorageData도 백업 용도로만 사용한다.
            // 그러니 바로바로 UserData에 접근해서 사용하자.
            UserStorageData storageData = GameInstance.MainUser.storageData;
            if (storageData.cropStorage.TryGetValue(id, out Dictionary<ECropGrade, int> slot) == false)
                return false;

            if (slot.TryGetValue(grade, out int count) == false)
                return false;

            if(count < amount)
                return false;

            UserActionObserver.Invoke(EActionType.OwnCrop);
            UserActionObserver.TargetInvoke(EActionType.OwnTargetCrop, id);
            slot[grade] -= amount;
            return true;
        }

        public void StoreCrop(int id, ECropGrade grade, int amount)
        {
            // 우선은 StorageData도 백업 용도로만 사용한다.
            // 그러니 바로바로 UserData에 접근해서 사용하자.
            UserStorageData storageData = GameInstance.MainUser.storageData;
            if (storageData.cropStorage.TryGetValue(id, out Dictionary<ECropGrade, int> slot) == false)
                return;

            if (slot.ContainsKey(grade) == false)
                return;
                
            UserActionObserver.Invoke(EActionType.OwnCrop);
            UserActionObserver.TargetInvoke(EActionType.OwnTargetCrop, id);
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