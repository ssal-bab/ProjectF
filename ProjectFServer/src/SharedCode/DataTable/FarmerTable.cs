using System;
using H00N.DataTables;

namespace ProjectF.DataTables
{
    [Serializable]
    public class FarmerTableRow : DataTableRow
    {
        public float moveSpeed; // 최고 이동 속도
        public float health; // 쉬지않고 일할수 있는 양
        public float farmingSkill; // 한 번에 줄 수 있는 물의 정도
        public float adventureSkill; // 탐험에서 더 많은 아이템을 얻을 확률
        public string nameLocalKey;
    }

    public class FarmerTable : DataTable<FarmerTableRow> { }
}