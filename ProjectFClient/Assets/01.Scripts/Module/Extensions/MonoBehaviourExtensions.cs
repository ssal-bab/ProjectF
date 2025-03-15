using System;
using System.Collections;
using UnityEngine;

namespace H00N.Extensions
{
    public static class MonoBehaviourExtensions
    {
        public static IEnumerator DelayCoroutine(this MonoBehaviour self, float delay, Action callback)
        {
            yield return new WaitForSeconds(delay);
            callback?.Invoke();
        }

        public static IEnumerator YieldFrameCoroutine(this MonoBehaviour self, Action callback)
        {
            yield return null;
            callback?.Invoke();
        }

        public static IEnumerator LoopRoutine(this MonoBehaviour self, float delay, Action callback, float startDelay = 0f)
        {
            yield return new WaitForSeconds(startDelay);
            callback?.Invoke();

            YieldInstruction delayYieldInstruction = new WaitForSeconds(delay);
            while (true)
            {
                yield return delayYieldInstruction;
                callback?.Invoke();
            }
        }
    }
}