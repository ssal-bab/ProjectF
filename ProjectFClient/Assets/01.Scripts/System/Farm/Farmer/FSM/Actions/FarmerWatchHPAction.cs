using H00N.FSM;
using UnityEngine;

namespace ProjectF.Farms.AI
{
    public class FarmerWatchHPAction : FarmerFSMAction
    {
        [SerializeField] FSMState moveState = null;

        public override void UpdateState()
        {
            base.UpdateState();

            if(aiData.isResting)
                return;

            // hp가 0이하로 내려가면 모든 작업을 멈추고 숙소로 돌아간다.
            if(Farmer.Stat.CurrentHP > 0f)
                return;

            Farm currentFarm = new GetBelongsFarm(Farmer.transform).currentFarm;
            if(currentFarm == null)
            {
                brain.SetAsDefaultState();
                return;
            }

            aiData.isResting = true;
            aiData.PushTarget(currentFarm.FarmerQuarters);
            brain.ChangeState(moveState);
        }
    }
}
