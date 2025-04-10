using ProjectF.Datas;
using UnityEngine;

namespace ProjectF.Farms.AI
{
    public class FarmerPlantAction : FarmerAnimationAction
    {
        private Field currentField = null;

        public override void EnterState()
        {
            base.EnterState();

            currentField = aiData.CurrentTarget as Field;
            if(currentField.FieldState != EFieldState.Empty)
                brain.SetAsDefaultState();
        }

        protected override void OnHandleAnimationTrigger()
        {
            base.OnHandleAnimationTrigger();
            if(currentField.FieldState == EFieldState.Empty)
            {
                // 이미 데이터상으로는 심어진 상태이다. 데이터를 기반으로 초기화 해준다.
                FieldData fieldData = new GetFieldData(GameInstance.MainUser.fieldGroupData, currentField.FieldGroupID, currentField.FieldID).fieldData;
                if(fieldData != null)
                    currentField.Initialize(currentField.FieldGroupID, fieldData);
            }

            // 여기가 액션의 마지막이다. Target을 클리어한다.
            aiData.ClearTarget();
            Farmer.Stat.ReduceHP(10f);
        }

        protected override void OnHandleAnimationEnd()
        {
            base.OnHandleAnimationEnd();
            brain.SetAsDefaultState();
        }
    }
}
