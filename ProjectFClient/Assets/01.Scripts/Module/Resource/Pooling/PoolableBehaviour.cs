using System;
using UnityEngine;

namespace H00N.Resources.Pools
{
    public abstract class PoolableBehaviour : MonoBehaviour
    {
        public virtual void OnSpawned() { }
        public virtual void OnDespawn() { }
    }
}