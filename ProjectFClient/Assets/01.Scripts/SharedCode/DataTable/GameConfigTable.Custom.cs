namespace ProjectF.DataTables
{
    public partial class GameConfigTable : ConfigTable<GameConfigTableRow, float> 
    { 
        public int AdventureEggLootMaxCount => (int)GetValue("AdventureEggLootMaxCount");
    }
}
