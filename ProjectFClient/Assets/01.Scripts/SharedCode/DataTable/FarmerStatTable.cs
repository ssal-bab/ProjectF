using H00N.DataTables;
using ProjectF.Datas;

namespace ProjectF.DataTables
{
    public partial class FarmerStatTableRow : DataTableRow
    {
        public float[] moveSpeedStat;
        public float[] healthStat;
        public float[] farmingSkillStat;
        public float[] adventureSkillStat;
    }

    public partial class FarmerStatTable : DataTable<FarmerStatTableRow> { }
}
