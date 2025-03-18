using ProjectF.Datas;

namespace ProjectF.Farms.AI
{
    public class FarmerLoadAction : FarmerAnimationAction
    {
        private Storage currentStorage = null;

        public override void EnterState()
        {
            base.EnterState();

            currentStorage = aiData.CurrentTarget as Storage;
            if(currentStorage == null)
                brain.SetAsDefaultState();
        }

        protected override void OnHandleAnimationTrigger()
        {
            base.OnHandleAnimationTrigger();

            Crop crop = Farmer.HoldItem as Crop;
            if(crop != null)
                currentStorage.StoreCrop(crop.CropID, crop.CropGrade, 1);

            Farmer.ReleaseItem();
        }

        protected override void OnHandleAnimationEnd()
        {
            base.OnHandleAnimationEnd();
            brain.SetAsDefaultState();
        }
    }
}
