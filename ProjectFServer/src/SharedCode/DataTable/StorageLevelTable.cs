namespace ProjectF.DataTables
{
    public partial class StorageLevelTableRow : LevelTableRow
    {
        public int storeLimit;
        public float priceMultiplier;
    }

    public partial class StorageLevelTable : LevelTable<StorageLevelTableRow> { }
}
