using H00N.DataTables;

namespace ProjectF.DataTables
{
    public partial class AdventureEggLootTableRow : DataTableRow
    {
        public int adventureAreaID;
        public int adventureAreaLevel;
        public int eggID;
        public float rate;
    }

    public partial class AdventureEggLootTable : DataTable<AdventureEggLootTableRow> { }
}
