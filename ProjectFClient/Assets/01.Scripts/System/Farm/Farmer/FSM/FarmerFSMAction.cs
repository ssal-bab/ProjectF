using H00N.FSM;

namespace ProjectCoin.Farms.AI
{
    public abstract class FarmerFSMAction : FSMAction
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