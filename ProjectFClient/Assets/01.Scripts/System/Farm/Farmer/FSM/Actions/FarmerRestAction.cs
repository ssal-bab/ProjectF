namespace ProjectF.Farms.AI
{
    public class FarmerRestAction : FarmerFSMAction
    {
        public override void EnterState()
        {
            base.EnterState();

            FarmerQuarters farmerQuarters = aiData.CurrentTarget as FarmerQuarters;
            if(farmerQuarters == null)
            {
                brain.SetAsDefaultState();
                return;
            }

            // 여기가 액션의 마지막이다. Target을 클리어한다.
            aiData.ClearTarget();
            farmerQuarters.CageFarmer(Farmer.FarmerUUID);
        }
    }
}
