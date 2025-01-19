using System;
using H00N.DataTables;

namespace ProjectCoin.DataTables
{
    [Serializable]
    public class FarmerTableRow : DataTableRow
    {
        public float moveSpeed; // 최고 이동 속도
        public float iq; // ai 지능
        public float strength; // 한 번에 들 수 있는 정도
        public float luck; // 더 높은 등급의 작물이 나올 확률
        public float farmingSkill; // 한 번에 줄 수 있는 물의 정도
        public float harvestingSkill; // 더 많은 작물이 나올 확률
        public string nameLocalKey;
    }

    public class FarmerTable : DataTable<FarmerTableRow> { }
}