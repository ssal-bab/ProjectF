using H00N.FSM;
using UnityEngine;
using UnityEngine.Events;

namespace ProjectCoin.Farms.AI
{
    public class FarmerAnimationAction : FarmerFSMAction
    {
        [SerializeField] UnityEvent onAnimationStartEvent = null;
        [SerializeField] UnityEvent onAnimationTriggerEvent = null;
        [SerializeField] UnityEvent onAnimationEndEvent = null;
        protected FarmerAnimator animator = null;

        public override void Init(FSMBrain brain, FSMState state)
        {
            base.Init(brain, state);
            animator = brain.GetComponentInChildren<FarmerAnimator>();
        }

        public override void EnterState()
        {
            base.EnterState();
            animator.OnAnimationStartEvent += HandleAnimationStart;
            animator.OnAnimationTriggerEvent += HandleAnimationTrigger;
            animator.OnAnimationEndEvent += HandleAnimationEnd;
        }

        public override void ExitState()
        {
            base.ExitState();
            animator.OnAnimationStartEvent -= HandleAnimationStart;
            animator.OnAnimationTriggerEvent -= HandleAnimationTrigger;
            animator.OnAnimationEndEvent -= HandleAnimationEnd;
        }

        protected virtual void OnHandleAnimationStart() { }
        private void HandleAnimationStart()
        {
            OnHandleAnimationStart();
            onAnimationStartEvent?.Invoke();
        }

        protected virtual void OnHandleAnimationTrigger() { }
        private void HandleAnimationTrigger()
        {
            OnHandleAnimationTrigger();
            onAnimationTriggerEvent?.Invoke();
        }

        protected virtual void OnHandleAnimationEnd() { }
        private void HandleAnimationEnd()
        {
            OnHandleAnimationEnd();
            onAnimationEndEvent?.Invoke();
        }
    }
}