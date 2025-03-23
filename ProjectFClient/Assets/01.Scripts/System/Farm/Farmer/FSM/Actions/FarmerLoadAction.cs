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
                currentStorage.StoreCrop(crop.CropID, crop.CropGrade, crop.Count);

            // 여기가 액션의 마지막이다. Target을 클리어한다.
            aiData.ClearTarget();
            Farmer.Stat.ReduceHP(10f);
            Farmer.ReleaseItem();
        }

        protected override void OnHandleAnimationEnd()
        {
            base.OnHandleAnimationEnd();
            brain.SetAsDefaultState();
        }
    }
}
