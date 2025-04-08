using H00N.FSM;
using H00N.Resources.Pools;
using ProjectF.Datas;
using UnityEngine;

namespace ProjectF.Farms.AI
{
    public class FarmerPlantDecisionAction : FarmerFSMAction
    {
        [SerializeField] FSMState plantState = null;
        [SerializeField] FSMState moveState = null;

        public override void EnterState()
        {
            base.EnterState();

            Field currentField = aiData.CurrentTarget as Field;
            if (currentField.FieldState != EFieldState.Empty)
            {
                brain.SetAsDefaultState();
                return;
            }

            // 씨앗을 들고 왔으면 심고
            // 씨앗을 들고 오지 않았으면 씨앗을 가져오너라
            Item holdItem = Farmer.HoldItem;
            if(holdItem != null)
            {
                Farmer.ReleaseItem();
                PoolManager.Despawn(holdItem);

                brain.ChangeState(plantState);
                return;
            }

            Farm currentFarm = new GetBelongsFarm(Farmer.transform).currentFarm;
            if(currentFarm == null || currentFarm.CropQueue.CropQueueValid == false)
            {
                brain.SetAsDefaultState();
                return;
            }

            // 여기서는 사용된 척만 하고 실제 Dequeue 되는 곳은 심을 때여야 한다.
            // int cropID = currentFarm.CropQueue.DequeueCropData();
            // aiData.targetCropID = cropID;

            aiData.PushTarget(currentFarm.Storage);
            brain.ChangeState(moveState);
        }
    }
}
