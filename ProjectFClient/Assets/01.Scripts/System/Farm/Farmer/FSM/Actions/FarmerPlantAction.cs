using ProjectF.Datas;
using UnityEngine;

namespace ProjectF.Farms.AI
{
    public class FarmerPlantAction : FarmerAnimationAction
    {
        private Field currentField = null;
        private int seedCropID = -1;

        public override void EnterState()
        {
            base.EnterState();

            seedCropID = aiData.targetCropID;
            aiData.targetCropID = -1;

            currentField = aiData.CurrentTarget as Field;
            if(currentField.FieldState != EFieldState.Empty)
                brain.SetAsDefaultState();
        }

        protected override void OnHandleAnimationTrigger()
        {
            base.OnHandleAnimationTrigger();
            if(currentField.FieldState == EFieldState.Empty)
                currentField.Plant(seedCropID);

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
