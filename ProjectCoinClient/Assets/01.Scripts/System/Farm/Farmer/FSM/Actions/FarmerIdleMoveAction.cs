using H00N.Extensions;
using UnityEngine;

namespace ProjectCoin.Farms.AI
{
    public class FarmerIdleMoveAction : FarmerFSMAction
    {
        private Vector3 targetPosition = Vector3.zero;

        public override void EnterState()
        {
            base.EnterState();
            SetIdleTarget();
        }

        public override void UpdateState()
        {
            base.UpdateState();
        
            if((targetPosition - brain.transform.position).sqrMagnitude < 0.1f)
                SetIdleTarget();

            aiData.movement.SetDestination(targetPosition.PlaneVector());
        }

        private void SetIdleTarget()
        {
            targetPosition = Random.insideUnitCircle * 3f;
            targetPosition += transform.position;
        }
    }
}
