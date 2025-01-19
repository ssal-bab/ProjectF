using H00N.FSM;
using UnityEngine;

namespace ProjectCoin.Farms.AI
{
    public class FarmerLiftAction : FarmerAnimationAction
    {
        [SerializeField] FSMState moveState = null;
        private Item currentItem = null;

        public override void EnterState()
        {
            base.EnterState();

            currentItem = aiData.CurrentTarget as Item;
            if(currentItem == null)
                brain.SetAsDefaultState();
        }

        protected override void OnHandleAnimationTrigger()
        {
            base.OnHandleAnimationTrigger();

            Farmer.GrabItem(currentItem);
            aiData.PopTarget();
        }

        protected override void OnHandleAnimationEnd()
        {
            base.OnHandleAnimationEnd();

            // 이건 좀 더 고민해봐야 할 듯
            // 바닥에 떨어진 작물을 든 경우
            // 바닥에 떨어진 알을 든 경우
            // 둥지에 있던 알을 든 경우
            aiData.PushTarget(currentItem.DeliveryTarget);
            brain.ChangeState(moveState);
        }
    }
}
