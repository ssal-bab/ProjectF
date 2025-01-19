using H00N.FSM;

namespace ProjectCoin.Farms.AI
{
    public abstract class FarmerFSMDecision : FSMDecision
    {
        protected FarmerAIDataSO aiData = null;

        public override void Init(FSMBrain brain, FSMState state)
        {
            base.Init(brain, state);
            aiData = brain.GetFSMParam<FarmerAIDataSO>();
        }
    }
}