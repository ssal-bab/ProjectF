using System.Collections.Generic;
using H00N.Resources.Pools;
using UnityEngine;

namespace ProjectF.Farms
{
    public class FarmerTargetableBehaviour : PoolReference
    {
        private Dictionary<string, Farmer> watchers = null;

        public virtual Vector3 TargetPosition => transform.position;

        public bool IsWatched => watchers.Count > 0;

        protected override void Awake()
        {
            base.Awake();
            watchers = new Dictionary<string, Farmer>();
        }

        public virtual bool IsTargetEnable(Farmer farmer)
        {
            return true;
        }

        public Farmer GetWatcher(string farmerUUID)
        {
            watchers.TryGetValue(farmerUUID, out Farmer farmer);
            return farmer;
        }

        public void AddWatcher(Farmer watcher)
        {
            if(watchers.ContainsKey(watcher.FarmerUUID))
                return;

            watchers.Add(watcher.FarmerUUID, watcher);
        }

        public void RemoveWatcher(string farmerUUID)
        {
            watchers.Remove(farmerUUID);
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

            if(watchers == null)
                return;


            foreach(Farmer watcher in watchers.Values)
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