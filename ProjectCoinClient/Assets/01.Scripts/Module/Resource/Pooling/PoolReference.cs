using System.Collections.Generic;
using UnityEngine;

namespace H00N.Resources.Pools
{
    public class PoolReference : MonoBehaviour
    {
        private List<PoolableBehaviour> poolableBehaviours = null;
        
        private ResourceHandle handle = null;
        internal ResourceHandle Handle => handle;
        
        protected virtual void Awake()
        {
            poolableBehaviours = new List<PoolableBehaviour>();
            GetComponents<PoolableBehaviour>(poolableBehaviours);
        }

        internal void InitializeResource(ResourceHandle handle)
        {
            this.handle = handle;
        }

        public void Spawn()
        {
            poolableBehaviours.ForEach(i => i?.OnSpawned());
            OnSpawned();
        }

        protected virtual void OnSpawned() { }

        public void Despawn()
        {
            poolableBehaviours.ForEach(i => i?.OnDespawn());
            DespawnInternal();
        }

        protected virtual void DespawnInternal() { }
    }
}