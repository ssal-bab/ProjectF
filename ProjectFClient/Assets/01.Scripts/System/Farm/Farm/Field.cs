using System;
using Cysharp.Threading.Tasks;
using H00N.Resources;
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
        [SerializeField] PoolReference cropPrefab = null;

        public event Action<EFieldState> OnStateChangedEvent = null;
        public event Action<int> OnGrowUpEvent = null;

        private CropSO currentCropData = null;
        public CropSO CurrentCropData => currentCropData;

        private int fieldGroupID = 0;
        public int FieldGroupID => fieldGroupID;

        private int fieldID = 0;
        public int FieldID => fieldID;

        public int Growth = 0;
        public EFieldState FieldState = EFieldState.Fallow;

        private bool requestWaiting = false;
        private bool postponeTick = false;

        public override bool IsTargetEnable(Farmer farmer) 
        {
            // 이미 누군가 나를 타겟팅하고 있다면 Target할 수 없다.
            Farmer watcher = GetWatcher(farmer.FarmerUUID);
            if(watcher == null && IsWatched)
                return false;

            if(requestWaiting)
                return false;

            return FieldState != EFieldState.Growing;
        }

        public async void Initialize(int fieldGroupID, FieldData fieldData)
        {
            this.fieldGroupID = fieldGroupID;
            fieldID = fieldData.fieldID;
            FieldState = fieldData.fieldState;
            currentCropData = null;
             
            // 이미 심어져 있는 식물이 있는 경우 (작물 정보, 성장 상태)를 복원한다
            if(fieldData.currentCropID != -1)
            {
                currentCropData = await ResourceManager.LoadResourceAsync<CropSO>(ResourceUtility.GetCropSOKey(fieldData.currentCropID));
                Growth = fieldData.currentGrowth;
                OnGrowUpEvent?.Invoke(Growth / currentCropData.TableRow.growthStep);

                DateManager.Instance.OnTickCycleEvent += HandleTickCycleEvent;
            }

            ChangeState(FieldState);
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
