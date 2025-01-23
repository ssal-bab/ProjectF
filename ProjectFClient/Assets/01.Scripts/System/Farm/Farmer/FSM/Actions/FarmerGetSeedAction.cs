using H00N.FSM;
using H00N.Resources;
using H00N.Resources.Pools;
using UnityEngine;

namespace ProjectCoin.Farms.AI
{
    public class FarmerGetSeedAction : FarmerFSMAction
    {
        [SerializeField] FSMState liftState = null;

        public override void EnterState()
        {
            base.EnterState();

            CropSO targetSeedData = aiData.currentSeedData;
            Seed seed = PoolManager.Spawn("Seed") as Seed;
            seed.Initialize(targetSeedData.TableRow.seedItemID);
            seed.transform.position = brain.transform.position;

            // Storage로 향해있던 타겟을 제거하고 Seed로 변경
            aiData.PopTarget();
            aiData.PushTarget(seed);
            brain.ChangeState(liftState);
        }
    }
}
