using H00N.FSM;
using H00N.Resources;
using H00N.Resources.Pools;
using ProjectF.Farms.AI;
using ProjectF.Units;
using UnityEngine;

namespace ProjectF.Farms
{
    public class Farmer : PoolReference
    {
        [SerializeField] Transform grabPosition = null;

        // FarmerStat 추가하면서 주석처리
        //private FarmerStatSO statData = null;
        //public FarmerStatSO StatData => statData;

        private FarmerStat stat;
        public FarmerStat Stat => stat;

        private FSMBrain fsmBrain = null;
        private UnitMovement unitMovement = null;

        private Item holdItem = null;
        public Item HoldItem => holdItem;
        
        private FarmerAIDataSO aiData = null;
        public FarmerAIDataSO AIData => aiData;

        protected override void Awake()
        {
            base.Awake();

            unitMovement = GetComponent<UnitMovement>();
            fsmBrain = GetComponent<FSMBrain>();
        }

        public async void InitializeAsync(int id)
        {
            var loadedStatData = await ResourceManager.LoadResourceAsync<FarmerStatSO>($"FarmerStat_{id}");
            stat = new FarmerStat(loadedStatData.TableRow);

            unitMovement.SetMaxSpeed(stat[EFarmerStatType.MoveSpeed]);
            unitMovement.SetDestination(transform.position);

            fsmBrain.Initialize();

            aiData = fsmBrain.GetFSMParam<FarmerAIDataSO>();
            aiData.Initialize(this);

            fsmBrain.SetAsDefaultState();
        }

        public void GrabItem(Item item)
        {
            holdItem = item;
            holdItem.transform.SetParent(grabPosition);
            holdItem.transform.localPosition = Vector3.zero;

            holdItem.SetHolder(this);
        }

        public void ReleaseItem()
        {
            holdItem?.SetHolder(null);
            holdItem = null;
        }
    }
}
