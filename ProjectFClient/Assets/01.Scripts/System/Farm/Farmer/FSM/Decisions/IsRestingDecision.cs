namespace ProjectF.Farms.AI
{
    public class IsRestingDecision : FarmerFSMDecision
    {
        public override bool MakeDecision()
        {
            return aiData.isResting;
        }
    }
}