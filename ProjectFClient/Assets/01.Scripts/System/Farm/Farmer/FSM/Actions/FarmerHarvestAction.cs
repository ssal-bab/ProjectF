using Cysharp.Threading.Tasks;
using H00N.FSM;
using H00N.Resources.Pools;
using ProjectF.Datas;
using ProjectF.Networks;
using ProjectF.Networks.Packets;
using UnityEngine;

namespace ProjectF.Farms.AI
{
    public class FarmerHarvestAction : FarmerAnimationAction
    {
        [SerializeField] FSMState moveState = null;
        [SerializeField] PoolReference cropPrefab = null;

        private Field currentField = null;

        public override void EnterState()
        {
            base.EnterState();

            currentField = aiData.CurrentTarget as Field;
            if(currentField.FieldState != EFieldState.Fruition)
                brain.SetAsDefaultState();
        }

        protected override async void OnHandleAnimationTrigger()
        {
            base.OnHandleAnimationTrigger();
            if(currentField.FieldState != EFieldState.Fruition)
            {
                brain.SetAsDefaultState();
                return;
            }

            bool result = await brain.LockBrainAsync(async () => {
                Crop crop = await ProcessHarvestAsync(Farmer.FarmerUUID, currentField.FieldGroupID, currentField.FieldID);
                if(crop == null)
                {
                    brain.SetAsDefaultState();
                    return;
                }

                // 들고 바로 이동한다.
                Farmer.GrabItem(crop);

                Farm farm = FarmManager.Instance.MainFarm;
                aiData.PushTarget(farm.Storage);
                brain.ChangeState(moveState);
            });

            if(result == false)
            {
                brain.SetAsDefaultState();
                return;
            }
        }

        private async UniTask<Crop> ProcessHarvestAsync(string uuid, int fieldGroupID, int fieldID)
        {
            HarvestCropResponse response = await NetworkManager.Instance.SendWebRequestAsync<HarvestCropResponse>(new HarvestCropRequest(uuid, fieldGroupID, fieldID));
            if (response.result != ENetworkResult.Success)
                return null;

            ApplyHarvestCrop applyHarvestCrop = new ApplyHarvestCrop(GameInstance.MainUser.fieldGroupData, fieldGroupID, fieldID);
            if(applyHarvestCrop.fieldData == null)
                return null;

            currentField.Initialize(fieldGroupID, applyHarvestCrop.fieldData);

            Crop crop = PoolManager.Spawn<Crop>(cropPrefab);
            crop.transform.position = brain.transform.position;
            crop.Initialize(response.productCropID, response.cropGrade, response.cropCount);

            return crop;
        }
    }
}
