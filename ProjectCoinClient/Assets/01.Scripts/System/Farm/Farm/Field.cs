using System;
using Cysharp.Threading.Tasks;
using H00N.DataTables;
using H00N.Resources.Pools;
using ProjectCoin.Datas;
using ProjectCoin.DataTables;
using ProjectCoin.Farms.Helpers;
using ProjectCoin.Networks;
using ProjectCoin.Networks.Payloads;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ProjectCoin.Farms
{
    public class Field : FarmerTargetableBehaviour
    {
        [SerializeField] int fieldID = 0;

        public event Action<EFieldState> OnStateChangedEvent = null;
        public event Action<int> OnGrowUpEvent = null;

        private CropSO currentCropData = null;
        public CropSO CurrentCropData => currentCropData;

        // 나중엔 유저 정보에 갖고 있게 해야함
        private EFieldState currentState = EFieldState.None;
        public EFieldState CurrentState => currentState;

        private Farm currentFarm = null;

        private bool requestWaiting = false;
        private bool postponeTick = false;
        private int growth = 0;

        public override bool TargetEnable {
            get {
                if(requestWaiting)
                    return false;

                if(CurrentState == EFieldState.Empty)
                    return currentFarm.CropQueueValid;

                return CurrentState != EFieldState.Growing;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            currentFarm = new GetBelongsFarm(transform).currentFarm;
            ChangeState(EFieldState.Fallow);
        }

        public void Plant(CropSO cropData)
        {
            if (requestWaiting)
                return;

            currentCropData = cropData;
            PlantRequest payload = new PlantRequest(currentCropData.id, fieldID);
            NetworkManager.Instance.SendWebRequest<PlantResponse>(payload, HandlePlantResponse);
            requestWaiting = true;
        }

        private void HandlePlantResponse(PlantResponse res)
        {
            requestWaiting = false;
            if (res.networkResult != ENetworkResult.Success)
                return;

            growth = -1;
            ChangeState(EFieldState.Dried);
            GrowUp();

            DateManager.Instance.OnTickCycleEvent += HandleTickCycleEvent;
        }

        public void Harvest()
        {
            if (requestWaiting)
                return;

            DateManager.Instance.OnTickCycleEvent -= HandleTickCycleEvent;

            HarvestRequest payload = new HarvestRequest(fieldID);
            NetworkManager.Instance.SendWebRequest<HarvestResponse>(payload, HandleHarvestResponse);
            requestWaiting = true;
        }

        private async void HandleHarvestResponse(HarvestResponse res)
        {
            requestWaiting = false;
            if (res.networkResult != ENetworkResult.Success)
                return;

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

            if (currentState != EFieldState.Growing)
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

            if (currentStep >= currentCropData.TableRow.growthStep - 1)
                ChangeState(EFieldState.Fruition);
        }

        public void ChangeState(EFieldState targetState)
        {
            EFieldState prevState = currentState;
            currentState = targetState;
            if (prevState != currentState)
                OnStateChangedEvent?.Invoke(currentState);

            postponeTick = true;
        }
    }
}
