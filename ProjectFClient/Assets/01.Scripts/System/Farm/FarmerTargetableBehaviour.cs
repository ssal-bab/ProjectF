using H00N.Resources.Pools;
using UnityEngine;

namespace ProjectCoin.Farms
{
    public class FarmerTargetableBehaviour : PoolReference
    {
        private Farmer watcher = null;
        public Farmer Watcher => watcher;

        public virtual Vector3 TargetPosition => transform.position;

        public virtual bool TargetEnable => true;
        public bool IsWatched => watcher != false;

        public void SetWatcher(Farmer watcher)
        {
            this.watcher = watcher;
        }

        #if UNITY_EDITOR
        [Header("Editor")]
        public bool drawGizmos = true;
        public bool drawSelected = false;
        
        private void OnDrawGizmos()
        {
            if(drawGizmos == false)
                return;

            if(drawSelected && UnityEditor.Selection.activeObject != gameObject)
                return;

            DrawGizmos();
        }

        protected virtual void DrawGizmos()
        {
            Gizmos.color = Color.blue;
            for(float i = 0; i < 0.1; i += 0.01f)
            {
                Gizmos.DrawWireCube(TargetPosition, Vector3.one * (0.45f + i));
            }

            if(watcher != null)
            {
                Gizmos.color = Color.red;
                Vector3 direction = TargetPosition - watcher.transform.position;
                Vector3 normal = Vector3.Cross(Vector3.back, direction);
                Vector3 lineShiftDirection = normal.normalized;
                float theta = 0.01f;

                for(int i = -4; i < 5; ++i)
                {
                    Vector3 delta = lineShiftDirection * (theta * i);
                    Gizmos.DrawLine(watcher.transform.position + delta, TargetPosition + delta);
                }
            }
        }
        #endif
    }
}