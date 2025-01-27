using System;
using Cysharp.Threading.Tasks;
using H00N.DataTables;
using H00N.Resources.Pools;
using ProjectF.Datas;
using ProjectF.DataTables;
using ProjectF.Networks;
using ProjectF.Networks.Packets;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ProjectF.Farms
{
    public class Field : FarmerTargetableBehaviour
    {
        public event Action<EFieldState> OnStateChangedEvent = null;
        public event Action<int> OnGrowUpEvent = null;

        private CropSO currentCropData = null;
        public CropSO CurrentCropData => currentCropData;

        private FieldData fieldData = null;
        public FieldData FieldData => fieldData;

        private bool isDirty = false;
        public bool IsDirty => isDirty;

        public EFieldState CurrentState => fieldData.fieldState;
        public override bool TargetEnable {
            get {
                if(requestWaiting)
                    return false;

                if(CurrentState == EFieldState.Empty)
                    return currentFarm.CropQueue.CropQueueValid;

                return CurrentState != EFieldState.Growing;
            }
        }

        private Farm currentFarm = null;
        private bool requestWaiting = false;
        private bool postponeTick = false;
        private int growth = 0;

        public void Initialize(FieldData data)
        {
            fieldData = data;

            // 이미 심어져 있는 식물이 있는 경우 (작물 정보, 성장 상태)를 복원한다
            if(fieldData.currentCropID != -1)
            {
                currentCropData = new GetCropData(fieldData.currentCropID).currentData;
                growth = fieldData.currentGrowthStep * currentCropData.TableRow.growthRate;                
                OnGrowUpEvent?.Invoke(fieldData.currentGrowthStep);
            }

            currentFarm = new GetBelongsFarm(transform).currentFarm;
            ChangeState(fieldData.fieldState);
        }

        public async void Plant(CropSO cropData)
        {
            if (requestWaiting)
                return;

            currentCropData = cropData;
            requestWaiting = true;

            PlantRequest request = new PlantRequest(currentCropData.id, fieldData.fieldID);
            PlantResponse response = await NetworkManager.Instance.SendWebRequestAsync<PlantResponse>(request);
            
            requestWaiting = false;
            if (response.result != ENetworkResult.Success)
                return;

            fieldData.currentCropID = response.cropID;
            SetDirty();

            growth = -1;

            ChangeState(EFieldState.Dried);
            GrowUp();

            DateManager.Instance.OnTickCycleEvent += HandleTickCycleEvent;
        }

        public async void Harvest()
        {
            if (requestWaiting)
                return;

            DateManager.Instance.OnTickCycleEvent -= HandleTickCycleEvent;
            requestWaiting = true;

            HarvestRequest request = new HarvestRequest(fieldData.fieldID);
            HarvestResponse response = await NetworkManager.Instance.SendWebRequestAsync<HarvestResponse>(request);

            requestWaiting = false;
            if (response.result != ENetworkResult.Success)
                return;

            fieldData.currentGrowthStep = 0;
            SetDirty();

            UniTask task = CurrentCropData.TableRow.cropType switch {
                ECropType.Crop => SpawnCrop(),
                ECropType.Egg => SpawnFarmer(),
            };

            await task;
            ChangeState(EFieldState.Fallow);
        }

        private async UniTask SpawnFarmer()
        {
            Debug.Log($"We've got a new farmer!! : {currentCropData.TableRow.productCropID}");
        }

        private async UniTask SpawnCrop()
        {
            ItemTableRow tableRow = DataTableManager.GetTable<ItemTable>().GetRow(currentCropData.TableRow.productCropID);
            if (tableRow == null)
                return;

            Vector3 randomOffset = Random.insideUnitCircle * 3f;
            Vector3 itemPosition = TargetPosition + randomOffset;

            Item item = await PoolManager.SpawnAsync(tableRow.itemType.ToString()) as Item;
            item.transform.position = itemPosition;
            item.Initialize(tableRow.id);
        }

        private void HandleTickCycleEvent()
        {
            if (postponeTick)
            {
                postponeTick = false;
                return;
            }

            if (CurrentState != EFieldState.Growing)
                return;

            ChangeState(EFieldState.Dried);
            GrowUp();
        }

        private void GrowUp()
        {
            growth++;
            if (growth % currentCropData.TableRow.growthRate != 0)
                return;

            int currentStep = growth / currentCropData.TableRow.growthRate;
            OnGrowUpEvent?.Invoke(currentStep);

            fieldData.currentGrowthStep = currentStep;
            SetDirty();

            if (currentStep >= currentCropData.TableRow.growthStep - 1)
                ChangeState(EFieldState.Fruition);
        }

        public void ChangeState(EFieldState targetState)
        {
            EFieldState prevState = CurrentState;
            fieldData.fieldState = targetState;

            if (prevState != CurrentState)
            {
                OnStateChangedEvent?.Invoke(fieldData.fieldState);
                SetDirty();
            }

            postponeTick = true;
        }

        public void SetDirty()
        {
            isDirty = true;
        }

        public void ClearDirty()
        {
            isDirty = false;
        }
    }
}
