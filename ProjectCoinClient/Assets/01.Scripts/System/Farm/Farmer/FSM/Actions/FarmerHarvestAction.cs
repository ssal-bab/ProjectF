using ProjectCoin.Datas;

namespace ProjectCoin.Farms.AI
{
    public class FarmerHarvestAction : FarmerAnimationAction
    {
        private Field currentField = null;

        public override void EnterState()
        {
            base.EnterState();

            currentField = aiData.CurrentTarget as Field;
            if(currentField.CurrentState != EFieldState.Fruition)
                brain.SetAsDefaultState();
        }

        protected override void OnHandleAnimationTrigger()
        {
            base.OnHandleAnimationTrigger();
            if(currentField.CurrentState == EFieldState.Fruition)
                currentField.Harvest();
        }

        protected override void OnHandleAnimationEnd()
        {
            base.OnHandleAnimationEnd();
            brain.SetAsDefaultState();
        }
    }
}
