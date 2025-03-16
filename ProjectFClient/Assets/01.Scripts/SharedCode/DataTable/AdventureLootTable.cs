using H00N.DataTables;
using ProjectF.Datas;

namespace ProjectF.DataTables
{
    public partial class AdventureLootTableRow : DataTableRow
    {
        public float explorationTime;
        public int lootItemCount;
        public int lootItemCountDeviation;
        public int item1;
        public int item2;
        public int item3;
        public int item4;
        public int lootSeedCount;
        public int lootSeedCountDeviation;
        public int seed1;
        public int seed2;
        public int seed3;
    }

    public partial class AdventureLootTable : DataTable<AdventureLootTableRow> { }
}
