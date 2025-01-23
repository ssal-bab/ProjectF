using H00N.FSM;
using H00N.Resources;
using H00N.Resources.Pools;
using ProjectCoin.Farms.AI;
using ProjectCoin.Units;
using UnityEngine;

namespace ProjectCoin.Farms
{
    public class Farmer : PoolReference
    {
        [SerializeField] Transform grabPosition = null;

        private FarmerStatSO statData = null;
        public FarmerStatSO StatData => statData;

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
            statData = await ResourceManager.LoadResourceAsync<FarmerStatSO>($"FarmerStat_{id}");
            unitMovement.SetMaxSpeed(statData[EFarmerStatType.MoveSpeed]);
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
