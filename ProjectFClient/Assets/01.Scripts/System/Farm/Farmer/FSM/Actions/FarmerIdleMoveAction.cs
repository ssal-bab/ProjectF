using H00N.Extensions;
using UnityEngine;

namespace ProjectF.Farms.AI
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
            targetPosition = aiData.movement.GetValidDestination(targetPosition);
        }

        #if UNITY_EDITOR
        private bool drawGizmos = true;
        private bool drawSelected = false;
        
        private void OnDrawGizmos()
        {
            if(drawGizmos == false)
                return;

            if(drawSelected && UnityEditor.Selection.activeObject != gameObject)
                return;

            if(brain == null)
                return;

            if(brain.CurrentState != state)
                return;

            DrawGizmos();
        }

        protected virtual void DrawGizmos()
        {
            Gizmos.color = Color.blue;
            for(float i = 0; i < 0.1; i += 0.01f)
            {
                Gizmos.DrawWireCube(targetPosition, Vector3.one * (0.45f + i));
            }

            Gizmos.color = Color.red;
            Vector3 direction = targetPosition - transform.position;
            Vector3 normal = Vector3.Cross(Vector3.back, direction);
            Vector3 lineShiftDirection = normal.normalized;
            float theta = 0.01f;

            for(int i = -4; i < 5; ++i)
            {
                Vector3 delta = lineShiftDirection * (theta * i);
                Gizmos.DrawLine(transform.position + delta, targetPosition + delta);
            }
        }
        #endif
    }
}
