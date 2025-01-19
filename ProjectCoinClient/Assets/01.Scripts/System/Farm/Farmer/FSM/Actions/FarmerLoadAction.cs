namespace ProjectCoin.Farms.AI
{
    public class FarmerLoadAction : FarmerAnimationAction
    {
        private ItemStorage currentStorage = null;

        public override void EnterState()
        {
            base.EnterState();

            currentStorage = aiData.CurrentTarget as ItemStorage;
            if(currentStorage == null)
                brain.SetAsDefaultState();
        }

        protected override void OnHandleAnimationTrigger()
        {
            base.OnHandleAnimationTrigger();

            Item item = Farmer.HoldItem;
            currentStorage.StoreItem(item);
            Farmer.ReleaseItem();
        }

        protected override void OnHandleAnimationEnd()
        {
            base.OnHandleAnimationEnd();
            brain.SetAsDefaultState();
        }
    }
}
