using H00N.Extensions;
using UnityEngine;

namespace ProjectCoin.Farms.AI
{
    public class FarmerMoveAction : FarmerFSMAction
    {
        public override void UpdateState()
        {
            base.UpdateState();

            if(aiData.CurrentTarget == null)
                return;

            Vector3 targetPosition = aiData.CurrentTarget.TargetPosition;
            aiData.movement.SetDestination(targetPosition.PlaneVector());
        }
    }
}