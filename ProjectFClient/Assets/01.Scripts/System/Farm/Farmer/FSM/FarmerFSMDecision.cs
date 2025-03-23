using H00N.FSM;

namespace ProjectF.Farms.AI
{
    public abstract class FarmerFSMDecision : FSMDecision
    {
        protected FarmerAIDataSO aiData = null;
        protected Farmer Farmer => aiData.farmer;

        public override void Init(FSMBrain brain, FSMState state)
        {
            base.Init(brain, state);
            aiData = brain.GetFSMParam<FarmerAIDataSO>();
        }
    }
}