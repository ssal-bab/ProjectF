using H00N.DataTables;
using ProjectF.Datas;

namespace ProjectF.DataTables
{
    public partial class FarmerLevelupGoldTableRow : DataTableRow
    {
        public int CommonBaseValue;
        public int UnCommonBaseValue;
        public int RareBaseValue;
        public int EpicBaseValue;
        public int LegendaryBaseValue;
        public int MythicBaseValue;
        public float CommonMultiplierValue;
        public float UnCommonMultiplierValue;
        public float RareMultiplierValue;
        public float EpicMultiplierValue;
        public float LegendaryMultiplierValue;
        public float MythicMultiplierValue;
    }

    public partial class FarmerLevelupGoldTable : DataTable<FarmerLevelupGoldTableRow> { }
}
