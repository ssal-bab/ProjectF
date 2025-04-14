using H00N.DataTables;

namespace ProjectF.DataTables
{
    public partial class AdventureCropLootTableRow : DataTableRow
    {
        public int adventureAreaID;
        public int adventureAreaLevel;
        public int cropID;
        public int minValue;
        public int maxValue;
    }

    public partial class AdventureCropLootTable : DataTable<AdventureCropLootTableRow> { }
}
