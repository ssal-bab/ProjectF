using ProjectF.Datas;

namespace ProjectF.Farms.AI
{
    public class FarmerHarvestAction : FarmerAnimationAction
    {
        private Field currentField = null;

        public override void EnterState()
        {
            base.EnterState();

            currentField = aiData.CurrentTarget as Field;
            if(currentField.FieldState != EFieldState.Fruition)
                brain.SetAsDefaultState();
        }

        protected override void OnHandleAnimationTrigger()
        {
            base.OnHandleAnimationTrigger();
            if(currentField.FieldState == EFieldState.Fruition)
                currentField.Harvest(Farmer.FarmerUUID);

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
