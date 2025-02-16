using H00N.DataTables;
using H00N.FSM;
using H00N.Resources;
using H00N.Resources.Pools;
using ProjectF.Datas;
using ProjectF.DataTables;
using ProjectF.Farms.AI;
using ProjectF.Units;
using UnityEngine;

namespace ProjectF.Farms
{
    public class Farmer : PoolReference
    {
        [SerializeField] Transform grabPosition = null;

        // FarmerStat �߰��ϸ鼭 �ּ�ó��
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
            FarmerStatTableRow statTableRow = DataTableManager.GetTable<FarmerStatTable>().GetRow(id);
            stat = new FarmerStat(statTableRow);

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
