using System;
using System.Collections.Generic;
using H00N.Resources.Pools;
using UnityEngine;
using Object = UnityEngine.Object;

namespace H00N.Extensions
{
    public static class TransformExtensions
    {
        public static int DistanceCompare(this Transform transform, Transform a, Transform b)
        {
            float sqrDistanceA = (a.position - transform.position).sqrMagnitude;
            float sqrDistanceB = (b.position - transform.position).sqrMagnitude;
            return sqrDistanceA.CompareTo(sqrDistanceB);
        }

        public static void GetComponentsInChildren<T>(this Transform transform, List<T> result, bool includeSelf, bool recursive = true) where T : Component
        {
            if(includeSelf)
                result.AddRange(transform.GetComponents<T>());

            foreach(Transform child in transform)
            {
                result.AddRange(child.GetComponents<T>());
                if (recursive)
                    child.GetComponentsInChildren(result, false, true);
            }            
        }

        public static void DespawnAllChildren(this Transform transform)
        {
            foreach(Transform child in transform)
            {
                if(child.TryGetComponent<PoolReference>(out PoolReference poolReference) == false)
                    Object.Destroy(child.gameObject);
                else
                    PoolManager.Despawn(poolReference);
            }
        }
    }
}