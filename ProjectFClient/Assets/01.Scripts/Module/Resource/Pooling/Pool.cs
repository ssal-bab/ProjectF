using System.Collections.Generic;
using UnityEngine;

namespace H00N.Resources.Pools
{
    public class Pool
    {
        private Queue<PoolReference> pool = new Queue<PoolReference>();
        private PoolReference resource = null;

        public Pool(PoolReference resource)
        {
            this.resource = resource;
        }

        public void Release()
        {
            foreach(PoolReference instance in pool)
            {
                if(instance != null)
                    Object.Destroy(instance);
            }

            pool = null;
        }

        public PoolReference Spawn()
        {
            PoolReference instance = null;
            if(pool.Count > 0)
                instance = pool.Dequeue();
            else
                instance = Object.Instantiate(resource);
            
            instance.InitializeResource(resource.Handle);
            instance.Spawn();
            return instance;
        }

        public void Despawn(PoolReference instance)
        {
            instance.Despawn();
            pool.Enqueue(instance);
        }
    }
}
