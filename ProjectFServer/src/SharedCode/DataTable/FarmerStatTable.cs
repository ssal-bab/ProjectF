using H00N.DataTables;
using ProjectF.Datas;

namespace ProjectF.DataTables
{
    public partial class FarmerStatTableRow : DataTableRow
    {
        public float moveSpeedBaseValue;
        public float moveSpeedIncreaseValue;
        public float healthBaseValue;
        public float healthIncreaseValue;
        public float farmingSkillBaseValue;
        public float farmingSkillIncreaseValue;
        public float adventureSkillBaseValue;
        public float adventureSkillIncreaseValue;
    }

    public partial class FarmerStatTable : DataTable<FarmerStatTableRow> { }
}
