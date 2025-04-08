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

            Farm farm = new GetBelongsFarm(Farmer.transform).currentFarm;
            if(farm == null)
            {
                brain.SetAsDefaultState();
                return;
            }

            if(farm.CropQueue.CropQueueValid == false)
            {
                brain.SetAsDefaultState();
                return;
            }

            // 지금은 바로 Dequeue하지만 나중엔 Plant Request를 여기서 호출한다.
            // 서버상에서는 씨앗을 수령하는 순간 Queue에서 Dequeue됨과 동시에 밭에 심어지는 것.
            seed.Initialize(farm.CropQueue.DequeueCropData());
            seed.transform.position = brain.transform.position;

            // Storage로 향해있던 타겟을 제거하고 Seed로 변경
            aiData.PopTarget();
            aiData.PushTarget(seed);
            brain.ChangeState(liftState);
        }
    }
}
