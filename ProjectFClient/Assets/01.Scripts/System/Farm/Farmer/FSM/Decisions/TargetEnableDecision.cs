namespace ProjectF.Farms.AI
{
    public class TargetEnableDecision : FarmerFSMDecision
    {
        public override bool MakeDecision()
        {
            FarmerTargetableBehaviour currentTarget = aiData.CurrentTarget;
            if(currentTarget == null)
                return false;

            if(currentTarget.IsTargetEnable(Farmer) == false)
                return false;

            return true;
        }
    }
}
