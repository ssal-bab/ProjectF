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
                currentField.Harvest("ASD"); // 나중에 채워야 한다.
        }

        protected override void OnHandleAnimationEnd()
        {
            base.OnHandleAnimationEnd();
            brain.SetAsDefaultState();
        }
    }
}
