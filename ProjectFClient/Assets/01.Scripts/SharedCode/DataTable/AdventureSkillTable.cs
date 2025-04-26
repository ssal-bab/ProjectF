using H00N.DataTables;

namespace ProjectF.DataTables
{
    public partial class AdventureSkillTableRow : DataTableRow
    {
        public int level;
        public float additionalLootRate;
    }

    public partial class AdventureSkillTable : DataTable<AdventureSkillTableRow> { }
}
