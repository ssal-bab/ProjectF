using ProjectF.Datas;
using UnityEngine;

namespace ProjectF.Farms.AI
{
    public class FarmerFarmingAction : FarmerAnimationAction
    {
        [SerializeField] EFieldState fieldCondition = EFieldState.None;
        [SerializeField] EFieldState targetFieldState = EFieldState.None;

        private Field currentField = null;

        public override void EnterState()
        {
            base.EnterState();

            // State 들어올 때 조건에 맞지 않으면 액션을 취하지 않고 통과시킨다.
            currentField = aiData.CurrentTarget as Field;
            if(currentField.FieldState != fieldCondition)
                brain.SetAsDefaultState();
        }

        protected override void OnHandleAnimationTrigger()
        {
            base.OnHandleAnimationTrigger();

            // 여기까지 왔으면 조건이 맞지 않더라도 액션을 끊지 않는다. => 애니메이션 계속 진행
            if(currentField.FieldState == fieldCondition)
                currentField.ChangeState(targetFieldState);

            // 여기가 액션의 마지막이다. Target을 클리어한다.
            aiData.ClearTarget();
            Farmer.Stat.ReduceHP(10f);
        }

        protected override void OnHandleAnimationEnd()
        {
            base.OnHandleAnimationEnd();

            // idle 상태로 돌려버리기
            brain.SetAsDefaultState();
        }
    }
}
