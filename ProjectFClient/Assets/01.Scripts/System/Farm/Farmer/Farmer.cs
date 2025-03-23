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

        private FSMBrain fsmBrain = null;
        private UnitMovement unitMovement = null;

        public FarmerStat Stat { get; private set; } = null;
        public Item HoldItem { get; private set; } = null;
        public FarmerAIDataSO AIData { get; private set; } = null;
        public string FarmerUUID { get; private set; } = null;

        protected override void Awake()
        {
            base.Awake();

            unitMovement = GetComponent<UnitMovement>();
            fsmBrain = GetComponent<FSMBrain>();
        }

        public void Initialize(FarmerData farmerData)
        {
            RefreshData(farmerData);
            unitMovement.SetDestination(transform.position);

            fsmBrain.Initialize();

            AIData = fsmBrain.GetFSMParam<FarmerAIDataSO>();
            AIData.Initialize(this);

            fsmBrain.SetAsDefaultState();
        }

        public void RefreshData(FarmerData farmerData)
        {
            FarmerUUID = farmerData.farmerUUID;

            FarmerStatTableRow statTableRow = DataTableManager.GetTable<FarmerStatTable>().GetRow(farmerData.farmerID);
            Stat ??= new FarmerStat();
            Stat.SetData(statTableRow, farmerData.level);
            unitMovement.SetMaxSpeed(Stat[EFarmerStatType.MoveSpeed]);
        }

        public void GrabItem(Item item)
        {
            HoldItem = item;
            HoldItem.transform.SetParent(grabPosition);
            HoldItem.transform.localPosition = Vector3.zero;

            HoldItem.SetHolder(this);
        }

        public void ReleaseItem()
        {
            HoldItem?.SetHolder(null);
            HoldItem = null;
        }
    }
}
