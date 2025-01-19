using ProjectCoin.Datas;
using UnityEngine;

namespace ProjectCoin.Farms.AI
{
    public class FarmerPlantAction : FarmerAnimationAction
    {
        private Field currentField = null;
        private CropSO seedData = null;

        public override void EnterState()
        {
            base.EnterState();

            seedData = aiData.currentSeedData;
            aiData.currentSeedData = null;

            currentField = aiData.CurrentTarget as Field;
            if(currentField.CurrentState != EFieldState.Empty)
                brain.SetAsDefaultState();
        }

        protected override void OnHandleAnimationTrigger()
        {
            base.OnHandleAnimationTrigger();
            if(currentField.CurrentState == EFieldState.Empty)
                currentField.Plant(seedData);
        }

        protected override void OnHandleAnimationEnd()
        {
            base.OnHandleAnimationEnd();
            brain.SetAsDefaultState();
        }
    }
}
