namespace ProjectCoin.Farms.AI
{
    public class HoldItemTypeDecisionBase<T> : FarmerFSMDecision
    {
        public override bool MakeDecision()
        {
            return aiData.farmer.HoldItem is T;
        }
    }
}
