namespace ProjectF.DataTables
{
    public partial class AdventureLevelTableRow : LevelTableRow
    {
        public int adventureAreaID;
        public int adventureTime;
    }

    public partial class AdventureLevelTable : LevelTable<AdventureLevelTableRow> { }
}
