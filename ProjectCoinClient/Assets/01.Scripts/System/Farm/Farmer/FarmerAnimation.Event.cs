using System;
using UnityEngine;

namespace ProjectCoin.Farms
{
    public partial class FarmerAnimator
    {
        public event Action OnAnimationStartEvent = null;
        public event Action OnAnimationTriggerEvent = null;
        public event Action OnAnimationEndEvent = null;

        public void OnAnimationStart()
        {
            OnAnimationStartEvent?.Invoke();
        }

        public void OnAnimationTrigger()
        {
            OnAnimationTriggerEvent?.Invoke();
        }

        public void OnAnimationEnd()
        {
            OnAnimationEndEvent?.Invoke();
        }
    }
}