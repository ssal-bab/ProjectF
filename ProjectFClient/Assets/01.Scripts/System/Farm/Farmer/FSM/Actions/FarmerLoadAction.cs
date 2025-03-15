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

            Item item = Farmer.HoldItem;
            currentStorage.StoreCrop(item.ID, Datas.ECropGrade.None, 1);
            Farmer.ReleaseItem();
        }

        protected override void OnHandleAnimationEnd()
        {
            base.OnHandleAnimationEnd();
            brain.SetAsDefaultState();
        }
    }
}
