namespace ProjectCoin.Farms.AI
{
    public abstract class TargetTypeDecisionBase<T> : FarmerFSMDecision where T : FarmerTargetableBehaviour
    {
        public override bool MakeDecision()
        {
            return aiData.CurrentTarget is T;
        }
    }
}
