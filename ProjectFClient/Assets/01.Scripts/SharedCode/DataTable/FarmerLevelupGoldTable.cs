using H00N.DataTables;
using ProjectF.Datas;

namespace ProjectF.DataTables
{
    public partial class FarmerLevelupGoldTableRow : DataTableRow
    {
        public ERarity rarity;
        public int baseValue;
        public float multiplierValue;
    }

    public partial class FarmerLevelupGoldTable : DataTable<FarmerLevelupGoldTableRow> { }
}
