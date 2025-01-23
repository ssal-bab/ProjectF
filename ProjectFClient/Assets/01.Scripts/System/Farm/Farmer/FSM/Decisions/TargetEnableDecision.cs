namespace ProjectCoin.Farms.AI
{
    public class TargetEnableDecision : FarmerFSMDecision
    {
        public override bool MakeDecision()
        {
            FarmerTargetableBehaviour currentTarget = aiData.CurrentTarget;
            if(currentTarget == null)
                return false;

            if(currentTarget.TargetEnable == false)
                return false;

            if(currentTarget.IsWatched && aiData.farmer != currentTarget.Watcher)
                return false;

            return true;
        }
    }
}
