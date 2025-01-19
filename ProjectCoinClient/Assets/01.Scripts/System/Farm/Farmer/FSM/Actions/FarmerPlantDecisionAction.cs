using H00N.FSM;
using H00N.Resources.Pools;
using ProjectCoin.Datas;
using ProjectCoin.Farms.Helpers;
using UnityEngine;

namespace ProjectCoin.Farms.AI
{
    public class FarmerPlantDecisionAction : FarmerFSMAction
    {
        [SerializeField] FSMState plantState = null;
        [SerializeField] FSMState moveState = null;

        public override void EnterState()
        {
            base.EnterState();

            Field currentField = aiData.CurrentTarget as Field;
            if (currentField.CurrentState != EFieldState.Empty)
            {
                brain.SetAsDefaultState();
                return;
            }

            // 씨앗을 들고 왔으면 심고
            // 씨앗을 들고 오지 않았으면 씨앗을 가져오너라
            Item holdItem = aiData.farmer.HoldItem;
            if(holdItem != null)
            {
                aiData.farmer.ReleaseItem();
                PoolManager.Despawn(holdItem);

                brain.ChangeState(plantState);
                return;
            }

            Farm currentFarm = new GetBelongsFarm(aiData.farmer.transform).currentFarm;
            if(currentFarm == null || currentFarm.CropQueueValid == false)
            {
                brain.SetAsDefaultState();
                return;
            }

            CropSO targetCropData = currentFarm.DequeueCropData();
            aiData.currentSeedData = targetCropData;

            ItemStorage target = targetCropData.TableRow.cropType switch {
                ECropType.Egg => currentFarm.EggStorage,
                ECropType.Crop => currentFarm.CropStorage,
            };

            aiData.PushTarget(target);
            brain.ChangeState(moveState);
        }
    }
}
