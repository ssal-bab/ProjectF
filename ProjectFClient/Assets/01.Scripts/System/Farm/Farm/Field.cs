using System;
using H00N.Resources.Pools;
using ProjectF.Datas;
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

        private int fieldGroupID = 0;
        private int fieldID = 0;
        public int FieldID => fieldID;

        public int Growth = 0;
        public EFieldState FieldState = EFieldState.Fallow;

        public override bool TargetEnable {
            get {
                if(requestWaiting)
                    return false;

                if(FieldState == EFieldState.Empty)
                    return currentFarm.CropQueue.CropQueueValid;

                return FieldState != EFieldState.Growing;
            }
        }

        private Farm currentFarm = null;
        private bool requestWaiting = false;
        private bool postponeTick = false;

        public void Initialize(int fieldGroupID, FieldData fieldData)
        {
            this.fieldGroupID = fieldGroupID;
            fieldID = fieldData.fieldID;
            FieldState = fieldData.fieldState;
             
            // 이미 심어져 있는 식물이 있는 경우 (작물 정보, 성장 상태)를 복원한다
            if(fieldData.currentCropID != -1)
            {
                currentCropData = ResourceUtility.GetCropData(fieldData.currentCropID);
                Growth = fieldData.currentGrowth;
                OnGrowUpEvent?.Invoke(Growth / currentCropData.TableRow.growthStep);
            }

            currentFarm = new GetBelongsFarm(transform).currentFarm;
            ChangeState(FieldState);
        }

        public async void Plant(CropSO cropData)
        {
            if (requestWaiting)
                return;

            currentCropData = cropData;
            requestWaiting = true;

            PlantRequest request = new PlantRequest(currentCropData.id, fieldGroupID, fieldID);
            PlantResponse response = await NetworkManager.Instance.SendWebRequestAsync<PlantResponse>(request);
            
            requestWaiting = false;
            if (response.result != ENetworkResult.Success)
                return;

            currentCropData = ResourceUtility.GetCropData(response.cropID);
            Growth = -1;

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

            HarvestRequest request = new HarvestRequest(fieldGroupID, fieldID);
            HarvestResponse response = await NetworkManager.Instance.SendWebRequestAsync<HarvestResponse>(request);

            requestWaiting = false;
            if (response.result != ENetworkResult.Success)
                return;

            Growth = 0;

            SpawnCrop();
            ChangeState(EFieldState.Fallow);

            currentCropData = null;
        }

        private void SpawnCrop()
        {
            Vector3 randomOffset = Random.insideUnitCircle * 3f;
            Vector3 itemPosition = TargetPosition + randomOffset;

            Crop crop = PoolManager.Spawn<Crop>("Crop");
            crop.transform.position = itemPosition;
            crop.Initialize(currentCropData.TableRow.id);
        }

        private void HandleTickCycleEvent()
        {
            if (postponeTick)
            {
                postponeTick = false;
                return;
            }

            if (FieldState != EFieldState.Growing)
                return;

            ChangeState(EFieldState.Dried);
            GrowUp();
        }

        private void GrowUp()
        {
            Growth++;
            if (Growth % currentCropData.TableRow.growthRate != 0)
                return;

            int currentStep = Growth / currentCropData.TableRow.growthRate;
            OnGrowUpEvent?.Invoke(currentStep);

            if (currentStep >= currentCropData.TableRow.growthStep - 1)
                ChangeState(EFieldState.Fruition);
        }

        public void ChangeState(EFieldState targetState)
        {
            EFieldState prevState = FieldState;
            FieldState = targetState;

            if (prevState != FieldState)
                OnStateChangedEvent?.Invoke(FieldState);

            postponeTick = true;
        }
    }
}
