using System.Collections.Generic;
using UnityEngine;

namespace H00N.Resources.Pools
{
    public class PoolReference : MonoBehaviour
    {
        private List<IPoolableBehaviour> poolableBehaviours = null;
        
        private ResourceHandle handle = null;
        internal ResourceHandle Handle => handle;
        
        protected virtual void Awake()
        {
            poolableBehaviours = new List<IPoolableBehaviour>();
            GetComponents<IPoolableBehaviour>(poolableBehaviours);
        }

        internal void InitializeResource(ResourceHandle handle)
        {
            this.handle = handle;
        }

        public void Spawn()
        {
            poolableBehaviours.ForEach(i => {
                if(i != null)
                    i.OnSpawned();
            });
            OnSpawned();
        }

        protected virtual void OnSpawned() { }

        public void Despawn()
        {
            poolableBehaviours.ForEach(i => {
                if(i != null)
                    i.OnDespawn();
            });
            DespawnInternal();
        }

        protected virtual void DespawnInternal() { }
    }
}