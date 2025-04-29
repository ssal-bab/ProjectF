using H00N.DataTables;

namespace ProjectF.DataTables
{
    public partial class FarmerLevelTableRow : LevelTableRow
    {
        public int farmerID;
        public int upgradeMonetaCost;
        public int moveSpeed;
        public int health;
        public int farmingSkill;
        public int adventureSkill;
    }

    public partial class FarmerLevelTable : LevelTable<FarmerLevelTableRow> { }
}
