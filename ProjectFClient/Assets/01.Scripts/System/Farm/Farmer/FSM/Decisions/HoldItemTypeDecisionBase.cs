namespace ProjectF.Farms.AI
{
    public class HoldItemTypeDecisionBase<T> : FarmerFSMDecision
    {
        public override bool MakeDecision()
        {
            return Farmer.HoldItem is T;
        }
    }
}
