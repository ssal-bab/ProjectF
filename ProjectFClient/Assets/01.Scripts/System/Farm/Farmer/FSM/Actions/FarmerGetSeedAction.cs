using H00N.FSM;
using H00N.Resources.Pools;
using UnityEngine;

namespace ProjectF.Farms.AI
{
    public class FarmerGetSeedAction : FarmerFSMAction
    {
        [SerializeField] FSMState liftState = null;
        [SerializeField] PoolReference seedPrefab = null;

        public override void EnterState()
        {
            base.EnterState();

            Seed seed = PoolManager.Spawn<Seed>(seedPrefab);
            seed.Initialize(aiData.targetCropID);
            seed.transform.position = brain.transform.position;

            // Storage로 향해있던 타겟을 제거하고 Seed로 변경
            aiData.PopTarget();
            aiData.PushTarget(seed);
            brain.ChangeState(liftState);
        }
    }
}
